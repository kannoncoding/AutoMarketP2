/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase controladora para conectar el formulario principal del servidor con el servicio TCP de AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Windows.Forms;
using AutoMarket.Servidor.Comunicacion;

namespace AutoMarket.Servidor.Presentacion

{
    public sealed class ControladorServidor
    {
        private readonly FrmServidorPrincipal _frmServidorPrincipal;
        private readonly ServidorTcp _servidorTcp;

        public ControladorServidor(FrmServidorPrincipal frmServidorPrincipal)
        {
            _frmServidorPrincipal = frmServidorPrincipal ?? throw new ArgumentNullException(nameof(frmServidorPrincipal));
            _servidorTcp = new ServidorTcp();

            SuscribirEventos();
            ConfigurarVistaInicial();
        }

        private void SuscribirEventos()
        {
            _frmServidorPrincipal.SolicitudIniciarServidor += FrmServidorPrincipal_SolicitudIniciarServidor;
            _frmServidorPrincipal.SolicitudDetenerServidor += FrmServidorPrincipal_SolicitudDetenerServidor;
            _servidorTcp.EventoServidor += ServidorTcp_EventoServidor;
        }

        private void ConfigurarVistaInicial()
        {
            _frmServidorPrincipal.EstablecerDireccionEscucha(
                _servidorTcp.DireccionIp.ToString(),
                _servidorTcp.Puerto);

            _frmServidorPrincipal.ActualizarEstadoServidor(false, "🔴 Servidor detenido");
            _frmServidorPrincipal.EstablecerCantidadConexiones(0);
        }

        private void FrmServidorPrincipal_SolicitudIniciarServidor(object? sender, EventArgs e)
        {
            _servidorTcp.Iniciar();

            _frmServidorPrincipal.ActualizarEstadoServidor(true, "🟢 Servidor en ejecución");
            _frmServidorPrincipal.EstablecerDireccionEscucha(
                _servidorTcp.DireccionIp.ToString(),
                _servidorTcp.Puerto);
            _frmServidorPrincipal.EstablecerCantidadConexiones(_servidorTcp.CantidadClientesConectados);
            _frmServidorPrincipal.AgregarEventoBitacora("El administrador inició el servidor TCP.");
        }

        private void FrmServidorPrincipal_SolicitudDetenerServidor(object? sender, EventArgs e)
        {
            _servidorTcp.Detener();

            _frmServidorPrincipal.ActualizarEstadoServidor(false, "🔴 Servidor detenido");
            _frmServidorPrincipal.EstablecerCantidadConexiones(0);
            _frmServidorPrincipal.AgregarEventoBitacora("El administrador detuvo el servidor TCP.");
        }

        private void ServidorTcp_EventoServidor(string mensaje)
        {
            _frmServidorPrincipal.AgregarEventoBitacora(mensaje);
            _frmServidorPrincipal.EstablecerCantidadConexiones(_servidorTcp.CantidadClientesConectados);
        }

        public void DetenerServidorSiEstaActivo()
        {
            if (_servidorTcp.EstaEnEjecucion)
            {
                _servidorTcp.Detener();
            }
        }
    }
}