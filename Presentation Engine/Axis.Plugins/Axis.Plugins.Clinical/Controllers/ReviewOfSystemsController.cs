using Axis.Plugins.Clinical.Models.ReviewOfSystems;
using Axis.Plugins.Clinical.Repository.ReviewOfSystems;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Clinical.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsController : BaseController
    {
        #region Class Variables

        /// <summary>
        /// The review of systems repository
        /// </summary>
        private readonly IReviewOfSystemsRepository reviewOfSystemsRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsController"/> class.
        /// </summary>
        /// <param name="reviewOfSystemsRepository">The review of systems repository.</param>
        public ReviewOfSystemsController(IReviewOfSystemsRepository reviewOfSystemsRepository)
        {
            this.reviewOfSystemsRepository = reviewOfSystemsRepository;
        }

        #endregion Constructors

        #region Action Results

        /// <summary>
        /// Displays list of Review of Systems.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Reviews the of system header.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReviewOfSystemHeader()
        {
            return View();
        }

        /// <summary>
        /// Review.
        /// </summary>
        /// <returns></returns>
        public ActionResult Review()
        {
            return View();
        }

        #endregion Action Results
    }
}