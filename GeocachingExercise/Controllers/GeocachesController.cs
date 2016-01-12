using System.Net;
using System.Web.Mvc;
using GeocachingExercise.Models;
using GeocachingExercise.Persistence.EF;


namespace GeocachingExercise.Controllers
{
    public class GeocachesController : Controller
    {
        private IGeocacheRepository _geocacheRepository;

        public GeocachesController()
        {
            _geocacheRepository = new GeocacheRepository(new GeocacheContext());
        }

        public GeocachesController(IGeocacheRepository geocacheRepository)
        {
            _geocacheRepository = geocacheRepository;
        }

        // GET: Geocaches
        public ActionResult Index()
        {
            ViewBag.Title = "Geocaches Index";

            return View(_geocacheRepository.All());
        }

        // GET: Geocaches/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Details";

            if (id == null)
            {
                // No id provided in request
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Geocache geocache = _geocacheRepository.Find(id);

            if (geocache == null)
            {
                // No geocache found with this id
                return HttpNotFound();
            }

            // Valid geocache object found
            return View(geocache);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Create";

            return View();
        }

        // POST: Geocaches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Coordinate")] Geocache geocache)
        {
            if (ModelState.IsValid)
            {
                _geocacheRepository.Create(geocache);
                _geocacheRepository.Save();

                return RedirectToAction("Index");
            }

            // Display validation errors for invalid geocache object on Create.cshtml
            return View(geocache);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _geocacheRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
