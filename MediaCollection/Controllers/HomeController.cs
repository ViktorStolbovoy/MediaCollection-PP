using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection
{
	public class HomeController : MCControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult AddTitle()
		{
			return new EmptyResult();
		}

		public IActionResult UpdateTitleName()
		{
			return new EmptyResult();
		}

		public IActionResult ReparentTitle()
		{
			return new EmptyResult();
		}

		public IActionResult SetTitleOrder()
		{
			return new EmptyResult();
		}

		public IActionResult SetTitleLocation()
		{
			return new EmptyResult();
		}

		public IActionResult Search()
		{
			return View();
		}

		public IActionResult RescanLocation(int locationId)
		{
			return View();
		}

		public IActionResult Previos(int id)
		{
			return View();
		}

		public IActionResult Details(int id)
		{
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		public IActionResult Edit(int id)
		{
			return View();
		}

		[HttpPost]
		public IActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		public IActionResult Delete(int id)
		{
			return View();
		}

		[HttpPost]
		public IActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
