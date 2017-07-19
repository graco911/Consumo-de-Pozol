using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;

namespace PozolFrioApp
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        CustomAdapter adapter;
        ListView ListaConsumo;
        AzureDataService azuredataservice;
        List<PozolFrio> list;
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            CurrentPlatform.Init();

            Button btnRegistrar = FindViewById<Button>(Resource.Id.buttonRegistrarPozol);
            CheckBox checkAzucar = FindViewById<CheckBox>(Resource.Id.checkBoxPozolAzucar);
            ListaConsumo = FindViewById<ListView>(Resource.Id.listViewConsumoPozol);

            azuredataservice = new AzureDataService();
            await azuredataservice.Initialize();

            if (azuredataservice != null)
            {
                Refresh(azuredataservice);
            }

            btnRegistrar.Click += async delegate
            {
                await azuredataservice.AddPozol(checkAzucar.Checked);
                Refresh(azuredataservice);
            };
        }

        public async void Refresh(AzureDataService azuredataservice)
        {
            list = await azuredataservice.GetPozol();
            if (list != null)
            {
                adapter = new CustomAdapter(this,
                    list, 
                    Android.Resource.Layout.SimpleListItem2,
                    Android.Resource.Id.Text1,
                    Android.Resource.Id.Text2);
            }

            ListaConsumo.Adapter = adapter;
        }
    }
}

