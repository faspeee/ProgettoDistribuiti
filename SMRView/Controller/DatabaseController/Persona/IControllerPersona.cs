using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZyzzyvagRPC.Services;

namespace SMRView.Controller.ControllerContract
{
    public interface IControllerPersona
    {
        Task Read(int id);
        Task ReadAll();
        Task Insert(PersonagRPC persona);
        Task Update(PersonagRPC persona);
        Task Delete(int id);
    }
}
