﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gerador.Models;
using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Gerador.Filtros;

namespace Gerador.Controllers
{
	[FiltroPermissao]
    public class EmpreendimentosController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
		
        // GET: Empreendimentos
        public async Task<ActionResult> Index()
        {
			var usuario = db.Users.Find(User.Identity.GetUserId());
			var empresa = usuario.IDEmpresa;
			List<Empreendimentos> empreendimentos;

			if (User.IsInRole("Administrador") || User.IsInRole("Analista"))
			{
				empreendimentos = await db.Empreendimentos.Include(e => e.Empresas).ToListAsync();
			} else if(User.IsInRole("Gestor") || User.IsInRole("Usuario"))
			{
				empreendimentos = await db.Empreendimentos.Where(e => e.IDEmpresa ==  empresa).ToListAsync();
			} else
			{
				empreendimentos = null;
				return View("AcessoNegado");

			}
            
            return View(empreendimentos);
        }
		// GET: Quantidade de unidades
		public string QtdUnidadesLivres(int id)
		{
            var unidades = db.Unidades.Where(u => u.IDEmpreendimento == id && u.UnidadeStatus == Unidades.Status.Livre).ToArray();
			string totalUnidades = unidades.Length.ToString();
			return totalUnidades;
		}
        // GET: Quantidade de unidades
        public string QtdUnidadesAnalise(int id)
        {
            var unidades = db.Unidades.Where(u => u.IDEmpreendimento == id && u.UnidadeStatus == Unidades.Status.Análise).ToArray();
            string totalUnidades = unidades.Length.ToString();
            return totalUnidades;
        }
        // GET: Quantidade de unidades
        public string QtdUnidadesConcluida(int id)
        {
            var unidades = db.Unidades.Where(u => u.IDEmpreendimento == id && u.UnidadeStatus == Unidades.Status.Concluída).ToArray();
            string totalUnidades = unidades.Length.ToString();
            return totalUnidades;
        }
        // GET: Quantidade de unidades
        public string QtdUnidadesTotal(int id)
        {
            var unidades = db.Unidades.Where(u => u.IDEmpreendimento == id).ToArray();
            string totalUnidades = unidades.Length.ToString();
            return totalUnidades;
        }

        // GET: data do habite-se
        public string Habitese(int id)
		{
			var empreendimento = db.Empreendimentos.Find(id);
			string dataHabitese = empreendimento.DataEntrega.ToShortDateString();
			return dataHabitese;
		}

		// GET: Empreendimentos/Details/5
		public async Task<ActionResult> Details(int? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empreendimentos empreendimentos = await db.Empreendimentos.FindAsync(id);
            if (empreendimentos == null)
            {
                return HttpNotFound();
            }
            return View(empreendimentos);
        }

		// GET: Empreendimentos/Create
		[FiltroPermissao(Roles = "Administrador")]
		public ActionResult Create()
        {
            ViewBag.IDEmpresa = new SelectList(db.Empresas, "IDEmpresa", "Nome");
            return View();
        }

        // POST: Empreendimentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [FiltroPermissao(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IDEmpreendimento,Nome,DataEntrega,Produto,Campanha,IDEmpresa")] Empreendimentos empreendimentos)
        {
            if (ModelState.IsValid)
            {
                db.Empreendimentos.Add(empreendimentos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home", null);
            }

            ViewBag.IDEmpresa = new SelectList(db.Empresas, "IDEmpresa", "Nome", empreendimentos.IDEmpresa);
            return View(empreendimentos);
        }

        // GET: Empreendimentos/Edit/5
		[FiltroPermissao(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empreendimentos empreendimentos = await db.Empreendimentos.FindAsync(id);
            if (empreendimentos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDEmpresa = new SelectList(db.Empresas, "IDEmpresa", "Nome", empreendimentos.IDEmpresa);
            return View(empreendimentos);
        }

        // POST: Empreendimentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [FiltroPermissao(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IDEmpreendimento,Nome,DataEntrega,Produto,Campanha,IDEmpresa")] Empreendimentos empreendimentos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empreendimentos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home", null);
            }
            ViewBag.IDEmpresa = new SelectList(db.Empresas, "IDEmpresa", "Nome", empreendimentos.IDEmpresa);
            return View(empreendimentos);
        }

        // GET: Empreendimentos/Delete/5
		[FiltroPermissao(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empreendimentos empreendimentos = await db.Empreendimentos.FindAsync(id);
            if (empreendimentos == null)
            {
                return HttpNotFound();
            }
            return View(empreendimentos);
        }

        // POST: Empreendimentos/Delete/5
        [FiltroPermissao(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Empreendimentos empreendimentos = await db.Empreendimentos.FindAsync(id);
            db.Empreendimentos.Remove(empreendimentos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
