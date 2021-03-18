using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebApiMovieEx1.Data;
using WebApiMovieEx1.Models;

namespace WebApiMovieEx1.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiMovieEx1.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Movies>("Movies");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class MoviesController : ODataController
    {
        private MoviesDbContext db = new MoviesDbContext();

        // GET: odata/Movies
        [EnableQuery]
        public IQueryable<Movies> GetMovies()
        {
            return db.Movies;
        }

        // GET: odata/Movies(5)
        [EnableQuery]
        public SingleResult<Movies> GetMovies([FromODataUri] int key)
        {
            return SingleResult.Create(db.Movies.Where(movies => movies.MId == key));
        }

        // PUT: odata/Movies(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Movies> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movies movies = db.Movies.Find(key);
            if (movies == null)
            {
                return NotFound();
            }

            patch.Put(movies);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoviesExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(movies);
        }

        // POST: odata/Movies
        public IHttpActionResult Post(Movies movies)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movies);
            db.SaveChanges();

            return Created(movies);
        }

        // PATCH: odata/Movies(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Movies> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movies movies = db.Movies.Find(key);
            if (movies == null)
            {
                return NotFound();
            }

            patch.Patch(movies);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoviesExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(movies);
        }

        // DELETE: odata/Movies(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Movies movies = db.Movies.Find(key);
            if (movies == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movies);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MoviesExists(int key)
        {
            return db.Movies.Count(e => e.MId == key) > 0;
        }
    }
}
