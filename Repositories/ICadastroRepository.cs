﻿using CasaDoCodigo.Models;
using System.Collections.Generic;

namespace CasaDoCodigo.Repositories
{
    public interface ICadastroRepository
    {
        IList<Cadastro> GetCadastros();
    }
}