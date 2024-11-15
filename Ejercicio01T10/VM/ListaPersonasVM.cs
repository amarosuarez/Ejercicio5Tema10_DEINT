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
        #region Propiedades
        private clsPersona personaSeleccionada;
        private ObservableCollection<clsPersona> listaPersonas;
        private string busqueda;
        private DelegateCommand filtrarCommand;
        private DelegateCommand eliminarCommand;

        public clsPersona PersonaSeleccionada
        {
            get { return personaSeleccionada; }
            set
            {
                if (personaSeleccionada != value)
                {
                    personaSeleccionada = value;
                    eliminarCommand.RaiseCanExecuteChanged();
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<clsPersona> ListaPersonas
        {
            get { return listaPersonas; }
            set
            {
                listaPersonas = value;
            }
        }

        public string Busqueda
        {
            get => busqueda;
            set
            {
                busqueda = value;
                filtrarCommand.RaiseCanExecuteChanged();
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
            ListaPersonas = new ObservableCollection<clsPersona>(ClsListadoBL.listadoPersonas());

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
            var filteredList = ClsListadoBL.listadoPersonas()
                    .Where(p => p.Nombre.ToLower().Contains(Busqueda.ToLower()) ||
                                p.Apellidos.ToLower().Contains(Busqueda.ToLower()))
                    .ToList();

            // Limpiar la lista actual
            ListaPersonas.Clear();

            // Añadir solo los elementos filtrados
            foreach (var persona in filteredList)
            {
                ListaPersonas.Add(persona);
            }

            NotifyPropertyChanged("ListaPersonas");
        }

        /// <summary>
        /// Función que determina si el command se puede o no ejecutar
        /// </summary>
        /// <returns>Devuelve si el command se puede o no ejecutar</returns>
        public bool FiltrarCommand_CanExecute()
        {
            bool sePuedeBuscar = true;

            //Si no hay una persona seleccionada no se puede borrar

            if (string.IsNullOrEmpty(Busqueda))
            {
                sePuedeBuscar = false;
                ListaPersonas = new ObservableCollection<clsPersona>(ClsListadoBL.listadoPersonas());
                NotifyPropertyChanged("ListaPersonas");
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
        private void EliminarPersona()
        {
            if (PersonaSeleccionada != null)
            {
                ListaPersonas.Remove(PersonaSeleccionada);
                NotifyPropertyChanged("ListaPersonas");

                // Colocamos persona seleccionada como null ya que la hemos eliminado
                PersonaSeleccionada = null;
                NotifyPropertyChanged("PersonaSeleccionada");
            }
        }

        /// <summary>
        /// Función que determina si eliminar command se pude ejecutar o no
        /// </summary>
        /// <returns>Devuelve si eliminar command se pude ejecutar o no</returns>
        public bool EliminarCommand_CanExecute()
        {
            bool sePuedeEliminar = true;

            //Si no hay una persona seleccionada no se puede borrar

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
