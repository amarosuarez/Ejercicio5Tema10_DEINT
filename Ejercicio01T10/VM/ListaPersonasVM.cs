using BL;
using Ejercicio01T10.VM.Utils;
using ENT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ejercicio01T10.VM
{
    class ListaPersonasVM : INotifyPropertyChanged
    {
        #region Atributos
        private clsPersona personaSeleccionada;
        private ObservableCollection<clsPersona> listaPersonas;
        private ObservableCollection<clsPersona> listaPersonasFiltrada;
        private string busqueda;
        private DelegateCommand filtrarCommand;
        private DelegateCommand eliminarCommand;
        #endregion

        #region Propiedades
        public clsPersona PersonaSeleccionada
        {
            get { return personaSeleccionada; }
            set
            {
                if (personaSeleccionada != value)
                {
                    personaSeleccionada = value;
                    eliminarCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<clsPersona> ListaPersonasFiltrada
        {
            get { return listaPersonasFiltrada; }
        }

        public string Busqueda
        {
            get => busqueda;
            set
            {
                busqueda = value;

                if (string.IsNullOrEmpty(value))
                {
                    listaPersonasFiltrada = listaPersonas;
                    //listaPersonasFiltrada = new ObservableCollection<clsPersona>(listaPersonas);
                }

                filtrarCommand.RaiseCanExecuteChanged();
                NotifyPropertyChanged("ListaPersonasFiltrada");
            }
        }

        public DelegateCommand FiltrarCommand
        {
            get
            {
                return filtrarCommand;
            }
        }

        public DelegateCommand EliminarCommand
        {
            get
            {
                return eliminarCommand;
            }
        }
        #endregion

        #region Constructores
        public ListaPersonasVM()
        {
            // Inicializar la lista original
            listaPersonas = new ObservableCollection<clsPersona>(ClsListadoBL.listadoPersonas());
            listaPersonasFiltrada = new ObservableCollection<clsPersona>(listaPersonas);

            // Commands
            filtrarCommand = new DelegateCommand(FiltrarCommand_Executed,
                FiltrarCommand_CanExecute);
            eliminarCommand = new DelegateCommand(EliminarCommand_Executed,
                EliminarCommand_CanExecute);
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Función que se ejecuta cuando ejecutamos el command de filtrar
        /// </summary>
        private void FiltrarCommand_Executed()
        {
            FilterList();
        }

        /// <summary>
        /// Función que filtra la lista según lo buscado
        /// </summary>
        private void FilterList()
        {
            // Filtrar la lista según la búsqueda
            var filteredList = listaPersonas
                    .Where(p => p.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                                p.Apellidos.ToLower().Contains(busqueda.ToLower()));

            // Limpiar la lista filtrada
            listaPersonasFiltrada.Clear();

            // Añadir solo los elementos filtrados
            foreach (var persona in filteredList)
            {
                listaPersonasFiltrada.Add(persona);
            }

            NotifyPropertyChanged("ListaPersonasFiltrada");
        }

        /// <summary>
        /// Función que determina si el command se puede o no ejecutar
        /// </summary>
        /// <returns>Devuelve si el command se puede o no ejecutar</returns>
        public bool FiltrarCommand_CanExecute()
        {
            bool sePuedeBuscar = true;

            // Si no hay una búsqueda no se puede filtrar
            if (string.IsNullOrEmpty(busqueda))
            {
                sePuedeBuscar = false;
            }

            return sePuedeBuscar;
        }

        /// <summary>
        /// Función que se ejecuta cuando ejecutamos el command eliminar
        /// </summary>
        private void EliminarCommand_Executed()
        {
            EliminarPersona();
        }

        /// <summary>
        /// Función que elimina a la persona seleccionada de la lista
        /// </summary>
        private async void EliminarPersona()
        {
            if (personaSeleccionada != null)
            {
                // Mostrar el cuadro de diálogo de confirmación
                bool confirmar = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar eliminación",
                    "¿Estás seguro de que deseas eliminar a " + personaSeleccionada.Nombre + " " + PersonaSeleccionada.Apellidos + "?",
                    "Sí", "No");

                if (confirmar)
                {
                    // Eliminar la persona seleccionada
                    listaPersonas.Remove(personaSeleccionada);

                    // Volvemos a filtrar para que muestre los cambios
                    listaPersonasFiltrada.Remove(personaSeleccionada);

                    // Colocamos persona seleccionada como null ya que la hemos eliminado
                    personaSeleccionada = null;
                    NotifyPropertyChanged("PersonaSeleccionada");
                }
            }
        }

        /// <summary>
        /// Función que determina si eliminar command se puede ejecutar o no
        /// </summary>
        /// <returns>Devuelve si eliminar command se puede ejecutar o no</returns>
        public bool EliminarCommand_CanExecute()
        {
            bool sePuedeEliminar = true;

            // Si no hay una persona seleccionada no se puede borrar
            if (personaSeleccionada == null)
            {
                sePuedeEliminar = false;
            }

            return sePuedeEliminar;
        }
        #endregion

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
