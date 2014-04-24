using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;

namespace AgentEnEcoute
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// Le constructeur ScheduledAgent initialise le gestionnaire UnhandledException
        /// </remarks>
        static ScheduledAgent()
        {
            // S'abonner au gestionnaire d'exceptions prises en charge
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code à exécuter sur les exceptions non gérées
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Une exception non gérée s'est produite ; arrêt dans le débogueur
                Debugger.Break();
            }
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: ajoutez du code pour exécuter votre tâche en arrière-plan
            ShellTile tuileParDefaut = ShellTile.ActiveTiles.First();

            if (tuileParDefaut != null)
            {
                FlipTileData flipTileData = new FlipTileData
                {
                    BackContent = "Dernière MAJ " + DateTime.Now.ToShortTimeString(),
                    Count = new Random().Next(0, 20),
                };

                tuileParDefaut.Update(flipTileData);
            }

            NotifyComplete();
        }
    }
}