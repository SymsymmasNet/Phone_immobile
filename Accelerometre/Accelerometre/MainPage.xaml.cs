using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Accelerometre.Resources;
using System.Windows.Threading; 
using Microsoft.Devices.Sensors;
using Windows.Phone.Speech.Recognition;

namespace Accelerometre
{
    public partial class MainPage : PhoneApplicationPage
        {
            SpeechRecognizerUI recoWithUI;

            private Accelerometer accelerometre;
            int timer_active_ecoute = 0;
            float val_x;
            float val_y;
            float val_z;

            float save_val_x;
            float save_val_y;
            float save_val_z;

            private const string NomTache = "MajHeure";

            public MainPage()
            {
                InitializeComponent();
                accelerometre = new Accelerometer();
                accelerometre.CurrentValueChanged += accelerometre_CurrentValueChanged;
                accelerometre.Start();

                affiche_statut.Text = "En attente";

                // creating timer instance
                DispatcherTimer newTimer = new DispatcherTimer();
                // timer interval specified as 1 second
                newTimer.Interval = TimeSpan.FromSeconds(1);
                // Sub-routine OnTimerTick will be called at every 1 second
                newTimer.Tick += OnTimerTick;
                // starting the timer
                newTimer.Start();
            }

            void OnTimerTick(Object sender, EventArgs args)
            {
                // text box property is set to current system date.
                // ToString() converts the datetime value into text
                Heure.Text = DateTime.Now.Second.ToString();
                if(timer_active_ecoute==0)
                {
                    save_val_x = val_x;
                    save_val_y = val_y;
                    save_val_z = val_z;

                    save_val_x_text.Text = save_val_x.ToString();
                    save_val_y_text.Text = save_val_y.ToString();
                    save_val_z_text.Text = save_val_z.ToString();

                    timer_active_ecoute = timer_active_ecoute + 1;
                }
                else
                {
                    val_x_text.Text = val_x.ToString();
                    val_y_text.Text = val_y.ToString();
                    val_z_text.Text = val_z.ToString();

                    if((save_val_x >= val_x-0.03) && (save_val_x <= val_x+0.03) && (save_val_y >= val_y-0.03) && (save_val_y <= val_y+0.03) && (save_val_z >= val_z-0.03) && (save_val_z <= val_z+0.03))
                    {
                        affiche_statut.Text = "Immobile";
                        timer_active_ecoute = timer_active_ecoute + 1;
                    }
                    else
                    {
                        timer_active_ecoute = 0;
                    }

                    if (timer_active_ecoute == 0)
                    {
                        affiche_statut.Text = "En mouvement";
                    }
                }

                if(timer_active_ecoute==5)
                {
                    Heure2.Text = "vide";
                    Hearing();
                }
                //else
                //{
                    //Heure2.Text = "";
                //}
            }

        
            private void accelerometre_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
            {
                Dispatcher.BeginInvoke(() => val_x = e.SensorReading.Acceleration.X);
                Dispatcher.BeginInvoke(() => val_y = e.SensorReading.Acceleration.Y);
                Dispatcher.BeginInvoke(() => val_z = e.SensorReading.Acceleration.Z);

                Dispatcher.BeginInvoke(() => Valeurs.Text = e.SensorReading.Acceleration.X + ", " + e.SensorReading.Acceleration.Y + ", " + e.SensorReading.Acceleration.Z);

            }

            private async void Hearing()
            {
                SpeechRecognitionUIResult recoResult;

                this.recoWithUI = new SpeechRecognizerUI();   // Creates an instance of SpeechRecognizerUI.
                recoResult = await recoWithUI.RecognizeWithUIAsync();   // Starts recognition (loads the dictation grammar by default).

                if (recoResult.RecognitionResult.Text != null)
                    Heure2.Text = recoResult.RecognitionResult.Text;

            }


        }

        

    }
