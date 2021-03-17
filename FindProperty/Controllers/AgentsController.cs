using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindProperty.Data;
using FindProperty.Models;
using FindProperty.Controllers;
using Microsoft.WindowsAzure.Storage.Blob;
using FindProperty.Views.Properties;
using Microsoft.AspNetCore.Http;

namespace FindProperty.Views.Agents
{
    public class AgentsController : Controller
    {
        private readonly FindProperty1Context _context;
        private BlobsController blobsController;

        public AgentsController(FindProperty1Context context)
        {
            _context = context;
            blobsController = new BlobsController();
        }

        // GET: Agents
        public async Task<IActionResult> Index(string searchString)
        {
            var agents = await _context.Agent.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                agents = agents.Where(x => x.name == searchString || x.phone_number == searchString).ToList();
            }

            foreach (var agent in agents)
            {
                agent.profile_picture = blobsController.getBlockBlobs(agent.profile_picture).First().Uri.ToString();
            }
            return View(agents);
        }

        // GET: Agents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agent.Include(x => x.Properties)
                .FirstOrDefaultAsync(m => m.AgentID == id);
            if (agent == null)
            {
                return NotFound();
            }
            agent.profile_picture = blobsController.getBlockBlobs(agent.profile_picture).First().Uri.ToString();

            agent.Properties.ForEach(property =>
                blobsController.getBlockBlobs(property.imagePath).ToList().ForEach(blob => property.images.Add(blob.Uri.ToString()))
            );

            return View(agent);
        }

        // GET: Agents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AgentId,name,phone_number,profilePicture")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                agent.profile_picture = blobsController.uploadBlob(agent.profilePicture);
                _context.Add(agent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }

        // GET: Agents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agent.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }
            agent.profilePreview = blobsController.getBlockBlobs(agent.profile_picture).First().Uri.ToString();
            return View(agent);
        }

        // POST: Agents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AgentID,name,phone_number,profilePicture,profile_picture")] Agent agent)
        {
            if (id != agent.AgentID)
            {
                return NotFound();
            }
            ModelState.Remove("profilePicture");
            if (ModelState.IsValid)
            {
                try
                {
                    if(agent.profilePicture != null)
                    {
                        blobsController.deleteBlobContainer(agent.profile_picture);
                        agent.profile_picture = blobsController.uploadBlob(agent.profilePicture);
                    }                   
                    _context.Update(agent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgentExists(agent.AgentID))
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
            return View(agent);
        }

        // GET: Agents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agent.Include(x => x.Properties)
                .FirstOrDefaultAsync(m => m.AgentID == id);
            if (agent == null)
            {
                return NotFound();
            }

            agent.profile_picture = blobsController.getBlockBlobs(agent.profile_picture).First().Uri.ToString();

            agent.Properties.ForEach(property =>
                blobsController.getBlockBlobs(property.imagePath).ToList().ForEach(blob => property.images.Add(blob.Uri.ToString()))
            );
            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {           
            var agent = await _context.Agent.Include(x=> x.Properties).FirstOrDefaultAsync(m => m.AgentID == id); ;
            if(agent.Properties.Count != 0)
            {
                agent.profile_picture = blobsController.getBlockBlobs(agent.profile_picture).First().Uri.ToString();

                agent.Properties.ForEach(property =>
                    blobsController.getBlockBlobs(property.imagePath).ToList().ForEach(blob => property.images.Add(blob.Uri.ToString()))
                );
                ViewBag.errorMessage = "Please ensures there is no properties in charged under this agent before removing.";
                return View(agent);
            }
            blobsController.deleteBlobContainer(agent.profile_picture);
            _context.Agent.Remove(agent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgentExists(int id)
        {
            return _context.Agent.Any(e => e.AgentID == id);
        }
    }
}
