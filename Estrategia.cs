
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
			List<string> listaUrls = new List<string>();

            if (arbol == null)
            {
                return listaUrls;
            }

            // Prefijo base del dominio
            string dominioBase = "tienda.com";

            // Recorrido DFS acumulando las partes del camino
            DfsUrls(arbol, dominioBase, listaUrls);

            return listaUrls;
		}

        private void DfsUrls(ArbolGeneral<ItemCat> nodo, string rutaActual, List<string> listaUrls){
            if (nodo == null) return;

            ItemCat dato = nodo.getDatoRaiz();
            string segmento = "";

            if (dato != null && !string.IsNullOrWhiteSpace(dato.Nombre))
            {
                // Limpiamos el texto a formato slug (minúsculas y reemplazo de espacios)
                segmento = dato.Nombre.Trim().ToLower().Replace(" ", "-");
            }

            // Armamos la URL acumulada hasta el nodo actual
            string nuevaRuta = string.IsNullOrEmpty(segmento) 
                ? rutaActual 
                : $"{rutaActual}/{segmento}";

            // Si es un nodo hoja, alcanzamos el final del camino y guardamos la URL
            if (nodo.esHoja())
            {
                listaUrls.Add(nuevaRuta);
                return;
            }

            // Si no es hoja, continuamos explorando sus subárboles hijos
            foreach (var hijo in nodo.getHijos())
            {
                DfsUrls(hijo, nuevaRuta, listaUrls);
            }
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

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre){
            if (rutaAlPadre == null || rutaAlPadre.Trim().Length == 0){
                throw new ArgumentException("La ruta al nodo padre no puede estar vacía.", nameof(rutaAlPadre));
            }

            string[] niveles = rutaAlPadre.Split(new char[] { '/', '>' }, StringSplitOptions.RemoveEmptyEntries);

            ArbolGeneral<ItemCat> actual = arbol;

            int inicio = 0;
            if (niveles.Length > 0 && actual.getDatoRaiz() != null &&
                actual.getDatoRaiz().Nombre.Equals(niveles[0].Trim(), StringComparison.OrdinalIgnoreCase))
            {
                inicio = 1;
            }

            for (int i = inicio; i < niveles.Length; i++){
                string nombreNivel = niveles[i].Trim();
                ArbolGeneral<ItemCat> hijoEncontrado = null;

            
                foreach (var hijo in actual.getHijos())
                {
                    if (hijo.getDatoRaiz() != null &&
                        hijo.getDatoRaiz().Nombre.Equals(nombreNivel, StringComparison.OrdinalIgnoreCase))
                    {
                        hijoEncontrado = hijo;
                        break;
                    }
                }

                if (hijoEncontrado == null)
                {
                    ItemCat nuevaCategoria = new ItemCat(nombreNivel, TipoElemento.Categoria);
                    hijoEncontrado = new ArbolGeneral<ItemCat>(nuevaCategoria);
                    actual.agregarHijo(hijoEncontrado);
                }

                actual = hijoEncontrado;
            }

            actual.agregarHijo(new ArbolGeneral<ItemCat>(dato));
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar){
			List<ItemCat> resultados = new List<ItemCat>();

            // Validación de precondiciones
            if (arbol == null || string.IsNullOrWhiteSpace(elementoABuscar))
            {
                return resultados;
            }

            // Normalizamos el término de búsqueda una sola vez
            string texto = elementoABuscar.Trim();

            // Ejecutamos el recorrido en profundidad (DFS)
            DfsBuscar(arbol, texto, resultados);

            return resultados;
        }

        private void DfsBuscar(ArbolGeneral<ItemCat> nodo, string texto, List<ItemCat> resultados)
        {
            if (nodo == null) return;

            ItemCat actual = nodo.getDatoRaiz();

            // 1. Visitar nodo actual: verificamos si coincide total o parcialmente
            if (actual != null && !string.IsNullOrEmpty(actual.Nombre))
            {
                if (actual.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
                {
                    resultados.Add(actual);
                }
            }

            // 2. Profundizar: llamada recursiva hacia los hijos (DFS)
            foreach (var hijo in nodo.getHijos())
            {
                DfsBuscar(hijo, texto, resultados);
            }
        }
            
    }
}