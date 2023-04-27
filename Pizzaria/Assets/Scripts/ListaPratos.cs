using UnityEngine;
using System;
using System.IO;

[Serializable]
public class NodoPrato {
    public Prato prato;
    public NodoPrato proximo;

    public NodoPrato(Prato prato) {
        this.prato = prato;
        proximo = null;
    }
}

[Serializable]
public class ListaPratos {
    public NodoPrato primeiro, ultimo;

    public ListaPratos() {
        primeiro = ultimo = new NodoPrato(null);
    }

    public void Adicionar(Prato prato) {
        NodoPrato novo = new NodoPrato(prato);
        ultimo.proximo = novo;
        ultimo = novo;
    }

    public Prato Remover(string nome) {
        NodoPrato atual = primeiro;
        while (atual.proximo != null) {
            if (atual.proximo.prato.nome == nome) {
                NodoPrato removido = atual.proximo;
                atual.proximo = atual.proximo.proximo;
                return removido.prato;
            }
            atual = atual.proximo;
        }
        return null;
    }

    public Prato Pesquisar(string nome) {
        NodoPrato atual = primeiro;
        while (atual.proximo != null) {
            if (atual.proximo.prato.nome == nome) {
                return atual.proximo.prato;
            }
            atual = atual.proximo;
        }
        return null;
    }

    public void Limpar() {
        primeiro.proximo = null;
        ultimo = primeiro;
    }

    public int Tamanho() {
        int tamanho = 0;
        NodoPrato atual = primeiro;
        while (atual.proximo != null) {
            tamanho++;
            atual = atual.proximo;
        }
        return tamanho;
    }

    public Prato PratoAleatorio() {
        int tamanho = Tamanho();
        if (tamanho == 0) {
            return null;
        }
        int indice = UnityEngine.Random.Range(0, tamanho);
        NodoPrato atual = primeiro;
        for (int i = 0; i <= indice; i++) {
            atual = atual.proximo;
        }
        return atual.prato;
    }

    
    public static ListaPratos LerPratosArquivo(ListaPratos listaPratos) {
        // Formato do arquivo: nome;preco;molho;tempoDePreparo;ingrediente1,quantidade1;ingrediente2,quantidade2;...

        listaPratos.Limpar();

        StreamReader arquivo = new StreamReader("Assets/Scripts/pratos.txt");
        string linha = arquivo.ReadLine();

        while (linha != null) {
            string[] dados = linha.Split(';');
            string nome = dados[0];
            float preco = float.Parse(dados[1]);
            string molho = dados[2];
            int tempoDePreparo = int.Parse(dados[3]);

            QuantIngredientes[] ingredientes = new QuantIngredientes[dados.Length - 4];
            for (int i = 4; i < dados.Length; i++) {
                string[] ingrediente = dados[i].Split(',');
                string nomeIngrediente = ingrediente[0];
                int quantidade = int.Parse(ingrediente[1]);
                ingredientes[i - 4] = new QuantIngredientes(nomeIngrediente, quantidade);
            }

            Prato prato = new Prato(nome, preco, molho, tempoDePreparo, ingredientes);
            listaPratos.Adicionar(prato);
            linha = arquivo.ReadLine();
        }

        arquivo.Close();
        return listaPratos;
    }
}
