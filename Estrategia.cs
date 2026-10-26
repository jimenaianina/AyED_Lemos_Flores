
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
            if (arbol == null)
            {
                return string.Empty;
            }

            string dominioBase = "tienda.com";
            string? resultado = DfsUrlPorId(arbol, dominioBase, id);

            if (resultado != null)
            {
                return resultado;
            }

            return string.Empty;
        }
        private string? DfsUrlPorId(ArbolGeneral<ItemCat> nodo, string rutaActual, int id){
            if (nodo == null){
                return null;
            }

            ItemCat dato = nodo.getDatoRaiz();
            string segmento = string.Empty;

            if (dato != null && !string.IsNullOrWhiteSpace(dato.Nombre)){
                segmento = dato.Nombre.Trim().ToLower().Replace(" ", "-");
            }

            // Evitamos duplicar si la raíz ya tiene de nombre "tienda" o "tienda.com"
            string nuevaRuta;
            if (rutaActual == "tienda.com" && (segmento.Contains("tienda") || string.IsNullOrEmpty(segmento))){
                nuevaRuta = rutaActual;
            }else{
                nuevaRuta = string.IsNullOrEmpty(segmento) ? rutaActual : $"{rutaActual}/{segmento}";
            }

            // Si encontramos el elemento por su Id, retornamos inmediatamente la URL
            if (dato != null && dato.Id == id){
                return nuevaRuta;
            }

            // Búsqueda en profundidad en los subárboles hijos
            foreach (var hijo in nodo.getHijos()){
                string? resultadoHijo = DfsUrlPorId(hijo, nuevaRuta, id);
                if (resultadoHijo != null)
                {
                    return resultadoHijo; // Propaga el resultado hacia arriba y corta la búsqueda
                }
            }

            return null; // No se encontró en esta rama
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
            string nuevaRuta;
            // Si estamos en la raíz y su nombre ya es la tienda o el dominio, evitamos duplicarlo
            if (rutaActual == "tienda.com" && (segmento.Contains("tienda") || string.IsNullOrEmpty(segmento)))
            {
                nuevaRuta = rutaActual;
            }
            else
            {
                nuevaRuta = string.IsNullOrEmpty(segmento) ? rutaActual : $"{rutaActual}/{segmento}";
            }

            // Condición de corte (nodo hoja): alcanzamos el final del camino y guardamos la URL completa
            if (nodo.esHoja())
            {
                listaUrls.Add(nuevaRuta);
                return;
            }

            // Si no es hoja, continuamos explorando recursivamente sus subárboles hijos
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

            /* Verifica si el dato ya existe dentro del padre encontrado
             Lo que venia sucediendo antes era que verificaba su ruta "Celulares/Iphone/Elem" lo que hacia la
             verificacion de la ruta anterior a Elem, por lo cual si yo ejecutase 2 veces "Celulares/Iphone/Elem"
             la verificacion de la ruta sucede de forma excelente, pero el Elem se duplica dentro de Iphone.*/
            bool yaExisteDato = false;
            foreach (var hijo in actual.getHijos()){
                if (hijo.getDatoRaiz() != null &&
                    hijo.getDatoRaiz().Nombre.Equals(dato.Nombre, StringComparison.OrdinalIgnoreCase))
                {
                    yaExisteDato = true;
                    break;
                }
            }

            if (!yaExisteDato)
            {
                actual.agregarHijo(new ArbolGeneral<ItemCat>(dato));
            }
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

        /*Un arbol,en este caso general, es un grafo aciclico dirigido lo cual el recorrido PreOrden
        lo hace de igual manera, primero la raiz y luegos sus descendientes, lo unico que cambia de usar un DFS
        en un grafo es que no utlizamos un array Visitados, que en este caso no nos sirve usarlo debido a que 
        ya la estructura del arbol te asegura que no va a ver un ciclo y que termino siendo un recorrido infinito*/
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