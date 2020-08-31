using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGSCIMB.Data;
using CGSCIMB.Models;
using CGSCIMB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CGSCIMB.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly ApplicationDbContext db;


        public FileController(FileService fileService, ApplicationDbContext db)
        {
            _fileService = fileService;
            this.db = db;
        }

        // download file(s) to client according path: rootDirectory/subDirectory with single zip file
        [HttpGet("Download/{subDirectory}")]
        public IActionResult DownloadFiles(string subDirectory)
        {
            try
            {
                var (fileType, archiveData, archiveName) = _fileService.FetechFiles(subDirectory);

                return File(archiveData, fileType, archiveName);
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadFile([FromForm] FileInfo fi)
        {
            try
            {
                var file = new File()
                {
                    ArticleTitle = fi.ArticleTitle,
                    Category = fi.Category,
                    Description = fi.Description,
                    fileName = fi.files[0].FileName,
                    countReader = 0

                };
                db.File.Add(file);
                db.SaveChanges();

                _fileService.SaveFile(fi.files, null);


                return Ok(new { fi.files.Count, Size = FileService.SizeConverter(fi.files.Sum(f => f.Length)) });
                //return RedirectToAction("Index", "Home");

            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }

        [HttpGet("GetData")]
        public IActionResult GetFiles()
        {

            var files = db.File.ToList<File>();
            return Ok(files);
        }

        [HttpPut("UpdateView/{id}")]
        public async Task<IActionResult> UpdateViewer(long id)
        {
            var exFile = db.File.Where(x => x.ID == id)
                                                    .FirstOrDefault<File>();
            if (exFile != null)
            {
                exFile.countReader += 1;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
            }
            else
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
