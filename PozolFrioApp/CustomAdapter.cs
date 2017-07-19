using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace PozolFrioApp
{
    [Activity(Label = "Consumo de Pozol", MainLauncher = true)]
    public class CustomAdapter : BaseAdapter<PozolFrio>
    {
        List<PozolFrio> PozolList;
        Activity Context;
        int ItemLayoutTemplate;
        int Date;
        int ConAzucar;

        public CustomAdapter(Activity context, List<PozolFrio> lista, int itemlayouttemplate, int date, int conazucar)
        {
            this.Context = context;
            this.PozolList = lista;
            this.ItemLayoutTemplate = itemlayouttemplate;
            this.Date = date;
            this.ConAzucar = conazucar;

        }

        public override PozolFrio this[int position]
        {
            get
            {
                return PozolList[position];
            }
        }

        public override int Count
        {
            get
            {
                return PozolList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return Convert.ToInt64(PozolList[position].id);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var Item = PozolList[position];
            View ItemView;
            if(convertView == null)
            {
                ItemView = Context.LayoutInflater.Inflate(ItemLayoutTemplate, null);
            }
            else
            {
                ItemView = convertView;
            }

            ItemView.FindViewById<TextView>(Date).Text = Item.DateUtc.ToString();
            ItemView.FindViewById<TextView>(ConAzucar).Text = Item.conAzucar == true ? "Con Azucar" : "Sin Azucar";

            return ItemView;
        }
    }
}