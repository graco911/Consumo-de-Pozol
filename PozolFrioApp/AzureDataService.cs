
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using PozolFrioApp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AzureDataService
{
    public MobileServiceClient MobileService { get; set; }
    public IMobileServiceSyncTable<PozolFrio> PozolTable;

    public async Task Initialize()
    {
        //creamos cliente para servicio en azure
        MobileService = new MobileServiceClient(@"https://granielazure.azurewebsites.net");
        //nombre del almacenamiento local
        const string path = "syncstore.db";
        //configuramos nuestro almacenamiento sqlite y agregamos el path
        var store = new MobileServiceSQLiteStore(path);
        //le pasamos el modelo para crear una tabla
        store.DefineTable<PozolFrio>();
        //vinculamos el almacenamiento local con el contexto
        await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
        //obtenemos la tabla local que luego se conectara a la remota en azure.
        PozolTable = MobileService.GetSyncTable<PozolFrio>();
    }

    public async Task<List<PozolFrio>> GetPozol()
    {
        SyncPozol();
        var list = await PozolTable.OrderByDescending(c => c.DateUtc).ToListAsync();
        return list;
    }

    public async Task AddPozol(bool conazucar)
    {
        //create and insert un elemento a la tabla local
        var pozol = new PozolFrio
        {
            DateUtc = DateTime.Now,
            conAzucar = conazucar
        };

        await PozolTable.InsertAsync(pozol);

        //Synchronize pozol
        try
        {
            await SyncPozol();
        }
        catch
        {
        }
    }

    public async Task SyncPozol()
    {
        //pull los ultimos registros y actualiza la tabla local
        await PozolTable.PullAsync("pozolitems", PozolTable.CreateQuery());
        await MobileService.SyncContext.PushAsync();
    }
}