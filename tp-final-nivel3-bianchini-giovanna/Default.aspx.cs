﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        public List<Articulo> ListaArticulos { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            ListaArticulos = negocio.Listar();
            if (!IsPostBack)
            {
                repRepetidor.DataSource = ListaArticulos;
                repRepetidor.DataBind();
            }
        }


        protected void btnFavoritos_Click(object sender, EventArgs e)
        {
            try
            {
                Trainee user = (Trainee)Session["trainee"];               
                if (user == null)
                {
                    // Manejar el caso donde el usuario no está autenticado
                    Session.Add("error", "Debes loguearte para agregar favoritos.");
                    Response.Redirect("Login.aspx", false);
                }
                
                FavoritoNegocio favoritoNegocio = new FavoritoNegocio();
                Favorito fav = new Favorito();

                fav.IdUser = user.Id;
                int idArticulo;
                string artId = ((Button)sender).CommandArgument;
               
                if (int.TryParse(artId, out idArticulo))
                {
                    
                    fav.IdArticulo = idArticulo;

                    int idFavorito = favoritoNegocio.InsertarNuevo(fav);
                    fav.Id = idFavorito;

                    Response.Redirect("Favoritos.aspx", false);
                }
                else
                {
                    // Manejar el caso donde CommandArgument no es un entero válido
                    // Mostrar mensaje de error o redirigir a una página de error
                    Session.Add("error", "El CommandArgument no es un número válido.");
                    Response.Redirect("Error.aspx", false);
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones específicas de manera más detallada si es necesario
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

    }
}