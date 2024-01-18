using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_REST.Models;
using Microsoft.AspNetCore.Cors;

namespace API_REST.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DbapiContext _dbcontext;

        public ProductoController(DbapiContext _context) { 
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista()
        {
            List <Producto> lista = new List<Producto>();

            try
            {
                lista = _dbcontext.Productos.Include(c => c.oCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]

        public IActionResult Obtener(int idProducto)
        {
            Producto oProducto = _dbcontext.Productos.Find(idProducto);
            if (oProducto == null)
            {
                return BadRequest("No encontrrado");
            }

            try
            {
                oProducto = _dbcontext.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = oProducto });
            }
        }

        [HttpPost]
        [Route("Guardar")]

        public IActionResult Guardar([FromBody] Producto objecto)
        {
            try
            {
                _dbcontext.Productos.Add(objecto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }


        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Producto objecto)
        {
            Producto oProducto = _dbcontext.Productos.Find(objecto.IdProducto);
            if (oProducto == null)
            {
                return BadRequest("No encontrrado");
            }

            try
            {
                oProducto.CodigoBarra = objecto.CodigoBarra is null ? oProducto.CodigoBarra : objecto.CodigoBarra;
                oProducto.Descripcion = objecto.Descripcion is null ? oProducto.Descripcion : objecto.Descripcion;
                oProducto.Marca = objecto.Marca is null ? oProducto.Marca : objecto.Marca;
                oProducto.Idcategoria = objecto.Idcategoria is null ? oProducto.Idcategoria : objecto.Idcategoria;
                oProducto.Precio = objecto.Precio is null ? oProducto.Precio : objecto.Precio;



                _dbcontext.Productos.Update(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]

        public IActionResult Eliminar(int idProducto)
        {
            Producto oProducto = _dbcontext.Productos.Find(idProducto);
            if (oProducto == null)
            {
                return BadRequest("No encontrrado");
            }

            try
            {
                _dbcontext.Productos.Remove(oProducto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
