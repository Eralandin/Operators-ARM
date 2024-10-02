using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Classes
{
    public class Tarif
    {
        private string tarifName;
        private float tarifPrice;
        private float innerCallPrice;
        private float outerCallPrice;
        private float smsPrice;

        public string TarifName
        {
            get { return tarifName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Название тарифа не может быть пустым!");
                }
                else
                {
                    tarifName = value;
                }
            }
        }
        public float TarifPrice
        {
            get { return tarifPrice; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Стоимость тарифа не может быть меньше нуля!");
                }
                else
                {
                    tarifPrice = value;
                }
            }
        }
        public float InnerCallPrice
        {
            get { return innerCallPrice; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Стоимость звонков внутри сети оператора не может быть меньше нуля!");
                }
                else
                {
                    innerCallPrice = value;
                }
            }
        }
        public float OuterCallPrice
        {
            get { return outerCallPrice; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Стоимость звонков вне сети оператора не может быть меньше нуля!");
                }
                else
                {
                    outerCallPrice = value;
                }
            }
        }
        public float SmsPrice
        {
            get { return smsPrice; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Стоимость СМС не может быть равна нулю или быть меньше!");
                }
                else
                {
                    smsPrice = value;
                }
            }
        }
    }
}
