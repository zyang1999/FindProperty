using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindProperty.Data;
using FindProperty.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using FindProperty.Controllers;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Cors;

namespace FindProperty.Views.Properties
{
    public class PropertiesController : Controller
    {
        private readonly FindProperty1Context _context;
        private BlobsController blobsController;

        public PropertiesController(FindProperty1Context context)
        {
            _context = context;
            blobsController = new BlobsController();
        }


        public async Task<IActionResult> Properties()
        {
            var properties = await _context.Property.ToListAsync();
            foreach (var property in properties)
            {
                setImages(property);
            }
            return View(properties);
        }

        // GET: Properties
        public async Task<IActionResult> Index(string propertyType, string searchString, string sort, string type)
        {
            var properties = await _context.Property.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                properties = properties.Where(s => s.title.Contains(searchString)).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                properties = await _context.Property.Where(s => s.address.Contains(searchString)).ToListAsync();
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                properties = properties.Where(x => x.property_type == propertyType).ToList();
            }

            if (!string.IsNullOrEmpty(type))
            {
                properties = properties.Where(x => x.type == type).ToList();
            }

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "newest":
                        properties = properties.OrderBy(property => property.created_at).ToList();
                        break;
                    case "highest price":
                        properties = properties.OrderByDescending(property => property.fee).ToList();
                        break;
                    case "lowest price":
                        properties = properties.OrderBy(property => property.fee).ToList();
                        break;
                    case "highest size":
                        properties = properties.OrderByDescending(property => property.size).ToList();
                        break;
                    case "lowest size":
                        properties = properties.OrderBy(property => property.size).ToList();
                        break;
                    default:
                        break;
                }
            }

            foreach (var property in properties)
            {
                setImages(property);
            }

            ViewBag.PropertyType = await _context.Property.Select(x => x.property_type).Distinct().ToListAsync();
            ViewData["searchString"] = searchString;
            ViewData["property_type"] = propertyType;
            ViewData["type"] = type;
            return View(properties);
        }

        //get images from blob storage
        public void setImages(Property property)
        {
            CloudBlobContainer container = blobsController.getBlobStorageInformation(property.imagePath);
            BlobResultSegment result = container.ListBlobsSegmentedAsync(null).Result;
            foreach (IListBlobItem item in result.Results)
            {
                property.images.Add(((CloudBlockBlob)item).Uri.ToString());
            }
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property.Include(x => x.Agent)
                .FirstOrDefaultAsync(m => m.id == id);
            if (@property == null)
            {
                return NotFound();
            }

            setImages(@property);

            @property.Agent.profile_picture = blobsController.getBlockBlobs(@property.Agent.profile_picture).First().Uri.ToString();

            return View(@property);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            ViewData["Agents"] = _context.Agent.ToList();
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,description,fee,size,type,furnishing,address,created_at,AgentID,property_type,imagesFiles")] Property @property)
        {
            if (ModelState.IsValid)
            {
                @property.imagePath = blobsController.uploadBlobs(@property.imagesFiles);
                _context.Add(@property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Agents"] = _context.Agent.ToList();
            return View(@property);
        }


        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }
            setImages(@property);
            ViewData["Agents"] = _context.Agent.ToList();
            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,description,fee,size,type,furnishing,address,status,imagePath,AgentID,created_at,property_type,imagesFiles")] Property @property)
        {
            if (id != @property.id)
            {
                return NotFound();
            }
            ModelState.Remove("imagesFiles");
            if (ModelState.IsValid)
            {
                try
                {
                    if (@property.imagesFiles != null)
                    {
                        @property.imagePath = blobsController.editBlob(@property.imagePath, @property.imagesFiles);
                    }
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Agents"] = _context.Agent.ToList();
            return View(@property);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property.Include(x => x.Agent)
                .FirstOrDefaultAsync(m => m.id == id);
            if (@property == null)
            {
                return NotFound();
            }
            setImages(@property);
            @property.Agent.profile_picture = blobsController.getBlockBlobs(@property.Agent.profile_picture).First().Uri.ToString();
            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Property.FindAsync(id);
            blobsController.deleteBlobContainer(@property.imagePath);
            _context.Property.Remove(@property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Property.Any(e => e.id == id);
        }

        [Route("Property/{id}")]
        public async Task<IActionResult> Properties_Detail(int id,string Message)
        {

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Message = Message;
            var @property = await _context.Property.Include(x => x.Agent)
                .FirstOrDefaultAsync(m => m.id == id);
            if (@property == null)
            {
                return NotFound();
            }

            setImages(@property);

            @property.Agent.profile_picture = blobsController.getBlockBlobs(@property.Agent.profile_picture).First().Uri.ToString();

            return View(@property);
        }

        public async Task<IActionResult> Search(string propertyType, string searchString)
        {
            var properties = await _context.Property.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                properties = await _context.Property.Where(s => s.title.Contains(searchString)).ToListAsync();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                properties = await _context.Property.Where(s => s.address.Contains(searchString)).ToListAsync();
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                properties = await _context.Property.Where(x => x.property_type == propertyType).ToListAsync();
            }

            foreach (var property in properties)
            {
                setImages(property);
            }

            ViewBag.PropertyType = new SelectList(await _context.Property.Select(x => x.property_type).ToListAsync());
            return View("Properties",properties);
        }

   
        
    }

}
