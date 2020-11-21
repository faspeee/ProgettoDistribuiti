using System.Threading.Tasks;

namespace SMRView.Controller.ControllerContract
{
    public interface IControllerMatematica
    {
        Task Factorial(int number);
        Task Fibonacci(int number);
    }
}
