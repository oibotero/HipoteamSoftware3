using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Proyecto_Integrador.Models;
using Proyecto_Integrador.Models.Dac_Entidades;
using Proyecto_Integrador.Services;


namespace Proyecto_Integrador.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private Contexto db = new Contexto();
        private ServiceUsuarios ServiceUsuario = new ServiceUsuarios();
        private Dac_Usuarios Dac_Usuarios = new Dac_Usuarios();
        private Dac_Roles Dac_Roles = new Dac_Roles();

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(Dac_Usuarios.ListaUsuarios());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuario = Dac_Usuarios.EncontrarUsuario(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Documento,Nombre,Apellido,Correo,Password")] Usuarios usuario)
        {

            if (Request.IsAuthenticated)
            {
                usuario.Rol = 2;
            }
            else
            {
                usuario.Rol = 3;
            }

            if (ModelState.IsValid)
            {
                usuario.Password = ServiceUsuario.Encripta(usuario.Password);
                Dac_Usuarios.GuardarUsuario(usuario);
                if (Request.IsAuthenticated)
                {
                    return RedirectToAction("Index");
                }
                else {
                    EnviarCorreo(usuario.Correo,usuario.Documento);
                    return RedirectToAction("SignIn");
                }
                
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuario = Dac_Usuarios.EncontrarUsuario(id); ;
            usuario.Password = ServiceUsuario.Desencripta(usuario.Password);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rol = new SelectList(db.Roles, "Id", "Nombre", usuario.Rol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Documento,Nombre,Apellido,Correo,Password,Rol")] Usuarios usuario)
        {
            
            if (ModelState.IsValid)
            {
                usuario.Password = ServiceUsuario.Encripta(usuario.Password);
                Dac_Usuarios.ActualizarUsuario(usuario);
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuario = Dac_Usuarios.EncontrarUsuario(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuario = Dac_Usuarios.EncontrarUsuario(id);
            Dac_Usuarios.EliminarUsuario(usuario);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "Correo,Password")] Login log)
        {
            
            Usuarios result = Dac_Usuarios.LoginUsuario(log);
            if (result != null)
            {
                string rol = Dac_Roles.EncontrarRol(result);
                if (result != null)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Documento.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, result.Nombre + "-" + rol));


                    AuthenticationManager.SignIn(identity);
                    Debug.WriteLine(User.Identity.GetUserName());
                    return RedirectToAction("IndexUsuario", "Productos");
                }

            }
            
                ViewData["resultado"] = "Error en Autenticacion";
                return View("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("IndexUsuario", "Productos");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dac_Usuarios.Cerrar();
            }
            base.Dispose(disposing);
        }

        private void EnviarCorreo(string correodestino,int id)
        {

            MailMessage email = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            email.To.Add(new MailAddress(correodestino));
            email.From = new MailAddress("xxxx.xxxxx@ucaldas.edu.co");
            email.Subject = "Proyecto Integrador Confirmacion de cuenta ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            email.SubjectEncoding = System.Text.Encoding.UTF8;
            email.Body = "<b>Confirma tu cuenta de la aplicacion Proyecto integrador aca:</b><br/>"+ "<a href='http://localhost:49300/Home/Confirmacion?iduser=" + id + "'>Enlace de Confirmación</a>";
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            //FileStream fs = new FileStream("E:\\TestFolder\\test.pdf", FileMode.Open, FileAccess.Read);
            //Attachment a = new Attachment(fs, "test.pdf", MediaTypeNames.Application.Octet);
            //email.Attachments.Add(a);

            smtp.Host = "smtp.gmail.com";  // IP empresa/institucional
                                          //smtp.Host = "smtp.hotmail.com";
                                          //smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("xxxxxxxx@gmail.com", "´password");

            //string lista = "ejemplo1@correo.com; ejemplo2@correo2.com;";
            //string output = string.Empty;

            //var mails = lista.Text.Split(';');
            //foreach (string dir in mails)
            //    email.To.Add(dir);

            try
            {
                smtp.Send(email);
                email.Dispose();
                Debug.WriteLine("Correo electrónico fue enviado satisfactoriamente.");
            }
           catch (Exception ex)
            {
                Debug.WriteLine("Error enviando correo electrónico: " + ex.Message);
               
            }
            
        }
    }
}
