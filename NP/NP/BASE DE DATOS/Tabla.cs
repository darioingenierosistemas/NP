using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace NP.BASE_DE_DATOS
{
    [Table("TABLA_CONSULTA")]
    public class TABLA_CONSULTA
    {

        private string m_CONSULTA;
        public string CONSULTA
        {

            get
            {
                return m_CONSULTA;
            }
            set
            {
                this.m_CONSULTA = value;
            }
        }

        private string m_NUMERO_PERFECTO;
        public string NUMERO_PERFECTO
        {

            get
            {
                return m_NUMERO_PERFECTO;
            }
            set
            {
                this.m_NUMERO_PERFECTO = value;
            }
        }

    }
}
