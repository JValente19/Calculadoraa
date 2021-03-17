using Calculadoraa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Calculadoraa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet] // facultativa
        public IActionResult Index(){

            // prepara os valores iniciais da View
            ViewBag.Visor = "0";
            ViewBag.PrimeiroOperador = "Sim";
            ViewBag.Operador = "";
            ViewBag.Primeirooperando = "";
            ViewBag.LimpaVisor = "Sim";

            return View();
        }

        /// <summary>
        /// Invocação da View, em modo POST
        /// </summary>
        /// <param name="botao">operador selecionado pelo utilizador</param>
        /// <param name="visor">valor do visor da calculadora</param>
        /// <param name="primeiroOperador">var auxiliar: Já foi escolhido o operador, ou não...</param>
        /// <param name="primeiroOperando">var auxiliar: Primeiro operando a ser utilizado na operação</param>
        /// <param name="operador">var auxiliar: Operador a ser utilizado na operação</param>
        /// <param name="limpaVisor">var auxiliar. se 'Sim' limpa visor, caso contrário não</param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult Index(string botao, 
            string visor, 
            string primeiroOperador, 
            string primeiroOperando, 
            string operador, 
            string limpaVisor){

     
            //avaliar o valor associado à variavel 'botao'
            switch (botao) {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    // atribuir ao 'visor' o valor do 'botao'
                    if (visor == "0"||limpaVisor == "Sim") visor = botao;
                    else visor = visor + botao; // visor +=botao;
                    // marcar q já não preciso de limpar o visor
                    limpaVisor = "Nao";
                    break;

                case "+/-":
                    // faz a inversão do valor do visor
                    if (visor.StartsWith('-')) visor = visor.Substring(1);
                    else visor = "-" + visor;
                    break;

                case ",":
                    // faz a gestão da parte decimal do numero do visor
                    if (visor.Contains(',')) visor += ",";
                    break;

                case "+":
                case "-":
                case "x":
                case ":":
                case "=":

                    limpaVisor = "Sim";
                    if (primeiroOperador != "Sim")
                    {
                        // esta é a 2º vez (ou mais) que se selecionou um 'operador'
                        //efetuar a operação com o operador antigo, e os valores dos operandos
                        double operando1 = Convert.ToDouble(primeiroOperando);
                        double operando2 = Convert.ToDouble(visor);
                        //efetuar a operação aritmétrica
                        switch (operador)
                        {
                            case "+":
                                visor = operando1 + operando2 + "";
                                break;
                            case "-":
                                visor = operando1 - operando2 + "";
                                break;
                            case "x":
                                visor = operando1 * operando2 + "";
                                break;
                            case ":":
                                visor = operando1 / operando2 + "";
                                break;
                        }
                    } // if (primeiroOperador != "Sim")

                      // armazenar os valores atuais para cálculos futuros
                      // visor atual, que passa a '1º operando'
                        primeiroOperando = visor;
                        // guardar o valor do operador para efetuar a operação
                        operador = botao;

                    // assinalar o que se vai fazer com os operadores
                    if (botao!="=") primeiroOperador = "Nao";
                    else primeiroOperador = "Sim";
                    
                    break;

                case "C":
                    // reset total da calculadora
                    visor = "0";
                    primeiroOperador = "Sim";
                    operador = "";
                    primeiroOperando = "";
                    limpaVisor = "Sim";

                    break;

            } // fim do switch

            //enviar o valor do 'visor' para a view
            ViewBag.Visor = visor;
            // preciso de manter o 'estado' das vars. auxiliares
            ViewBag.PrimeiroOperador = primeiroOperador;
            ViewBag.Operador = operador;
            ViewBag.Primeirooperando = primeiroOperando;
            ViewBag.LimpaVisor = limpaVisor;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
