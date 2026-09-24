
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
            List<List<string>> resultado = new List<List<string>>();
            if (arbol == null) return resultado;

            Cola<ArbolGeneral<ItemCat>> cola = new Cola<ArbolGeneral<ItemCat>>(); //Creamos la cola 
            cola.encolar(arbol); //Agregamos la raíz a la cola

            while (!cola.esVacia())//Mientras la cola no esté vacía, recorremos cada nivel
            {
                int cantidadEnNivel = cola.cantidadElementos();//Contamos cuántos nodos hay en este nivel
                List<string> elementosNivel = new List<string>();

                for (int i = 0; i < cantidadEnNivel; i++)//Procesamos todos los nodos de este nivel
                {
                    var nodoActual = cola.desencolar(); //Sacamos al primero de la cola
                    elementosNivel.Add(nodoActual.getDatoRaiz().Nombre); //Guardamos el nombre del nodo actual

                    foreach (var hijo in nodoActual.getHijos())//Encolamos cada hijo del nodo actual para el siguiente nivel
                    {
                        cola.encolar(hijo);
                    }
                }
                
                resultado.Add(elementosNivel);//Una vez recorrido el nivel entero, guardamos la lista de nodos en el resultado general
            }

            return resultado;
        }


        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> listaProductos = new List<ItemCat>();

            if (arbol == null) return listaProductos; // Si el árbol viene vacío, devolvemos la lista vacía

           
            if (arbol.getDatoRaiz().Tipo == TipoElemento.Producto) //Revisamos la raíz; si es un producto, lo guardamos en la lista
            {
                listaProductos.Add(arbol.getDatoRaiz());
            }

            foreach (var hijo in arbol.getHijos()) //Recorremos en profundidad cada uno de los hijos.
            {
                listaProductos.AddRange(Todos(hijo)); //Llamamos recursivamente al método y usamos AddRange para incluir en la lista todos los productos hijos 
            }

            return listaProductos;  //Devolvemos la lista completa con todos los productos encontrados
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