using Notes.Common.Models.Entities;
using Notes.Common.Repositories;
using Notes.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Notes.Controllers
{
    public class NoteController : Controller
    {
        private NoteRepository noteRepo = new NoteRepository();

        public ActionResult Index()
        {
            return View(noteRepo.Get());
        }

        public ActionResult Create()
        {
            return View(new Note());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Note note, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            note.ImageUrl = await AzureStorageUtil.UploadFileToAzureStorage(image);
            await noteRepo.Insert(note);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            return View(await noteRepo.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Note note, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string imageUrl = await AzureStorageUtil.UploadFileToAzureStorage(image);
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                note.ImageUrl = imageUrl;
            }
            await noteRepo.Update(id, note);
            return RedirectToAction("Index");
        }
    }
}