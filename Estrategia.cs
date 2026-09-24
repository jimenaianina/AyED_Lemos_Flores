
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Estrategia
	{
		
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            /*
            Retorna una cadena de texto con la URL amigable (SEO) correspondiente al elemento 
            almacenado en el árbol cuyo identificador coincide con el valor recibido como 
            parámetro.

            */
            
            return "Implementar";
        }
        

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
		{
            /*Retorna una List<string> que contiene todas las URLs amigables (SEO) generadas 
            a partir del árbol. Cada URL se construye recorriendo el camino desde la raíz hasta 
            cada hoja, concatenando el nombre de cada categoría. Por ejemplo:
            URL generada: tienda.com/electronica/computadoras/laptops/gaming
            */
			return ["Implementar"];
		}
        

              

        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
		{
            /*Retorna una List<List<string>> que contiene los elementos del árbol agrupados según 
            el nivel en el que se encuentran almacenados. Cada lista interna representa un nivel 
            del árbol, comenzando por la raíz.
            */
            return [["Implementar"]];
        }


        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> listaProductos = new List<ItemCat>();
    
            // Si el árbol viene vacío, devolvemos la lista vacía
            if (arbol == null) return listaProductos;

            //Revisamos el nodo actual (la raíz); si es un producto, lo guardamos en la lista
            if (arbol.getDatoRaiz().Tipo == TipoElemento.Producto)
            {
                listaProductos.Add(arbol.getDatoRaiz());
            }

            //Recorremos en profundidad cada uno de los hijos
            foreach (var hijo in arbol.getHijos())
            {
                // Llamamos recursivamente al método
                listaProductos.AddRange(Todos(hijo)); //usamos AddRange para incluir en la lista todos los productos hijos 
            }

            //Devolvemos la lista completa con todos los productos encontrados
            return listaProductos;
        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
		{
            /*Inserta un nuevo elemento en el árbol general. Tanto el árbol como el dato a 
            incorporar y la ruta correspondiente al nodo padre son recibidos como parámetros. Si la ruta
            indicada no existe, el método deberá crear automáticamente los nodos necesarios y, 
            posteriormente, insertar el nuevo elemento en dicha ubicación.*/
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
            /* Retorna una List<ItemCat> con todos los elementos del árbol cuyo nombre contenga, de forma 
            total o parcial, la cadena de texto recibida como parámetro.*/
			return [];
		}
            
    }
}