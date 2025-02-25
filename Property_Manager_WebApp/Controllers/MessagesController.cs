using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Property_Manager_WebApp.Models;

namespace Property_Manager_WebApp.Controllers
{
    public class MessagesController : Controller
    {
        private readonly PropertyRentalManagementContext _context;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(PropertyRentalManagementContext context, ILogger<MessagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User is not logged in. Redirecting to Login.");
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = int.Parse(userId);
            _logger.LogInformation("Fetching messages for UserId: {UserId}", currentUserId);

            var userMessages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .ToListAsync();

            _logger.LogInformation("Retrieved {MessageCount} messages for UserId: {UserId}", userMessages.Count, currentUserId);
            return View(userMessages);
        }



        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/CreateReply
        public IActionResult CreateReply(int recipientId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = int.Parse(userId);

            var message = new Message
            {
                SenderId = currentUserId,
                ReceiverId = recipientId
            };

            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "FullName", recipientId);
            return View("Create", message);
        }


        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "FullName");
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiverId,Content")] Message message)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User is not logged in. Redirecting to Login.");
                return RedirectToAction("Login", "Account");
            }

            message.SenderId = int.Parse(userId);
            message.SentAt = DateTime.Now;

            // Log message details
            _logger.LogInformation("Message creation attempt. SenderId: {SenderId}, ReceiverId: {ReceiverId}, Content: {Content}",
                message.SenderId, message.ReceiverId, message.Content);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(message);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Message successfully created with ID: {MessageId}", message.MessageId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while saving message.");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the message.");
                }
            }
            else
            {
                // Log model validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Model validation error: {ErrorMessage}", error.ErrorMessage);
                }
            }

            // Repopulate the dropdown for ReceiverId
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "FullName", message.ReceiverId);
            return View(message);
        }




        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "UserId", message.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "UserId", message.SenderId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,SenderId,ReceiverId,Content,SentAt")] Message message)
        {
            if (id != message.MessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageId))
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
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "UserId", message.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "UserId", message.SenderId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
