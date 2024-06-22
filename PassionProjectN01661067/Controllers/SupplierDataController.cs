using PassionProjectN01661067.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PassionProjectN01661067.Controllers
{
    public class SupplierDataController : ApiController
    {
        //GET: /api/SupplierData/ListSuppliers
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Suppliers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all suppliers in the database.
        /// </returns>
        /// <example>
        /// GET: api/SupplierData/ListSuppliers
        /// </example>

        [HttpGet]
        [Route("api/SupplierData/ListSuppliers")]
        public IEnumerable<SupplierDto> ListSuppliers()
        {
            List<Supplier> Suppliers = db.Suppliers.ToList();
            List<SupplierDto> SupplierDtos = new List<SupplierDto>();

            Suppliers.ForEach(a => SupplierDtos.Add(new SupplierDto()
            {
                //SupplierDto Item = new SupplierDto();
                SupplierId = a.SupplierId,
                SupplierName = a.SupplierName,
                ContactPerson = a.ContactPerson,
                EmailAddress = a.EmailAddress,
                SupplierAddress = a.SupplierAddress

            }));

            return SupplierDtos;
        }

        /// <summary>
        /// Finds a specific Supplier by ID.
        /// </summary>
        /// <param name="id">The ID of the Supplier.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Supplier matching the provided ID.
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/SupplierData/FindSupplier/5
        /// </example>
        //[HttpGet]
        //[Route("api/SupplierData/FindSupplier/{id}")]
        [ResponseType(typeof(Supplier))]
        [HttpGet]
        [Route("api/SupplierData/FindSupplier/{id}")]
        public IHttpActionResult FindSupplier(int id)
        {
            //Supplier Item = db.Suppliers.Find(id);


          

            Supplier Supplier = db.Suppliers.Find(id);

            SupplierDto SupplierDto = new SupplierDto()
            {
                SupplierId = Supplier.SupplierId,
                SupplierName = Supplier.SupplierName,
                ContactPerson = Supplier.ContactPerson,
                EmailAddress = Supplier.EmailAddress,
                SupplierAddress = Supplier.SupplierAddress

            };

            if (Supplier == null)
            {
                return NotFound();
            }


            return Ok(SupplierDto);
        }


        /// <summary>
        /// Updates a Supplier in the system with POST Data input.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be updated.</param>
        /// <param name="supplier">JSON form data of the Supplier.</param>
        /// <returns>
        /// HEADER: 204 (No Content)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/SupplierData/UpdateSupplier/5
        /// </example>
        // POST: api/SupplierData/UpdateSupplier/5
        [ResponseType(typeof(void))]
        [Route("api/SupplierData/UpdateSupplier/{id}")]
        [HttpPost]
        public IHttpActionResult UpdateSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.SupplierId)
            {

                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new Supplier to the system.
        /// </summary>
        /// <param name="supplier">JSON form data of the Supplier.</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: The created Supplier data.
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/SupplierData/AddSupplier
        /// </example>

        // POST: api/SupplierData/AddSupplier
        [ResponseType(typeof(Supplier))]
        [Route("api/SupplierData/AddSupplier")]
        [HttpPost]
        public IHttpActionResult AddSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Deletes a Supplier from the system by ID.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be deleted.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/SupplierData/DeleteSupplier/5
        /// </example>
        // POST: api/SupplierData/DeleteSupplier/5
        [ResponseType(typeof(Supplier))]
        [Route("api/SupplierData/DeleteSupplier/{id}")]
        [HttpPost]
        public IHttpActionResult DeleteSupplier(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.SupplierId == id) > 0;
        }
    }
}
