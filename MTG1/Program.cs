using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MTG1
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrice mat = new Matrice(1);
            bool again = true;
            Console.WriteLine(" ---------------------------\n| G-ToolBox V2 - (29/11/12) |\n ---------------------------\n");
            Console.WriteLine("1) Creer une matrice aleatoire a N sommets\n2) Importer un fichier CSV\n");
            int choix = int.Parse(Console.ReadLine());
            switch (choix)
            {
                case 1 :
                    Console.WriteLine("Nombre de sommets souhaité :");
                    int n = int.Parse(Console.ReadLine());
                    mat = new Matrice(n);
                    mat.fillMatrice();
                    break;

                case 2 :
                    Console.WriteLine("Chemin du fichier CSV :");
                    string chemin = Console.ReadLine();
                    mat = new Matrice(1);
                    mat.importCsv(chemin);
                    break;
            }


            Console.Clear();

            while (again)
            {
                Console.WriteLine(" ---------------------------\n| G-ToolBox V2 - (29/11/12) |\n ---------------------------\n");
                Console.WriteLine("Matrice d'adjacence :\n\n");
                mat.printMatrice();
                Console.WriteLine("\nActions :\n1) Ajouter un noeud\n2) Supprimer un noeud\n3) Modifier le nombre d'arc entre deux noeuds\n4) Parcours BFS\n5) Parcours DFS\n6) CFC selon Malgrange\n7) Warshall\n8) Quitter");
                int k = int.Parse(Console.ReadLine());

                switch (k)
                {
                    case 1 :
                        Console.WriteLine("\nLettre utilisee pour définir le nouveau noeud : ");
                        mat.addEdge(Console.ReadLine()[0]);
                        break;

                    case 2 :
                        Console.WriteLine("\nNoeud a supprimer : ");
                        mat.deleteEdge(Console.ReadLine()[0]);
                        break;

                    case 3 :
                        Console.WriteLine("\nExtremite 1 : ");
                        char edge1 = Console.ReadLine()[0];
                        Console.WriteLine("\nExtremite 2 : ");
                        char edge2 = Console.ReadLine()[0];
                        Console.WriteLine("\nNombre d'arc entre les deux extremites : ");
                        int nb = int.Parse(Console.ReadLine());
                        mat.setArcValue(edge1, edge2, nb);
                        break;

                    case 4:
                        Console.WriteLine("\nSommet racine : ");
                        List<char> pBFS = mat.BFS(mat.info.IndexOf(Console.ReadLine()[0]));
                        string parcours = "[ ";
                        foreach (char c in pBFS)
                        {
                            parcours += c + ", ";
                        }
                        parcours = parcours.Substring(0, parcours.Length - 2) + " ]";
                        Console.WriteLine(parcours);
                        Console.ReadLine();
                        break;

                    case 5:
                        Console.WriteLine("\nSommet racine : ");
                        List<char> pDFS = mat.DFS(mat.info.IndexOf(Console.ReadLine()[0]));
                        string parcours2 = "[ ";
                        foreach (char c in pDFS)
                        {
                            parcours2 += c + ", ";
                        }
                        parcours2 = parcours2.Substring(0, parcours2.Length - 2) + " ]";
                        Console.WriteLine(parcours2);
                        Console.ReadLine();
                        break;

                    case 6:
                        Console.WriteLine("Composante Fortement connexe du graphe :");
                        List<List<char>> CFC = mat.Malgrange();
                        string cfc = "[ ";
                        foreach (List<char> l in CFC)
                        {
                            cfc += "<";
                            foreach(char c in l)
                            {
                            cfc += c + ", ";
                            }
                            cfc += ">";
                        }
                        //cfc = cfc.Substring(0, cfc.Length - 2) + " ]";
                        Console.WriteLine(cfc);
                        Console.ReadLine();
                        break;

                    case 7:
                        int[,] war = mat.Warshall();
                        
                        for (int i = 0; i < mat.taille; i++)
                        {
                           for (int j = 0; j < mat.taille; j++)
                           {
                            Console.Write(war[i,j] + " ");
                           }
                           Console.Write("\n");
                        }

                        Console.ReadLine();
                        break;

                    case 8 :
                        again = false;
                        break;
                }
                Console.Clear();
            }
            
        }

    }

    class Matrice
    {
        static int M = int.MaxValue;
        public int taille;
        public int[,] tab;
        public List<char> info;
        static List<char> pDFS;
        static List<char> pred;

        //Instancie une matrice d'adjacence de taille n
        public Matrice(int n)
        {
            this.taille = n;
            this.tab = new int[n, n];
            this.info = new List<char>();
        }

        //Remplit aléatoirement une matrice (sans boucle sur les sommets)
        public void fillMatrice()
        {
            Random rand = new Random();

            for (int i = 0; i < this.taille; i++)
            {
                this.info.Add((char)(i + 65));
            }

            for (int ligne = 0; ligne < this.taille; ligne++)
            {
                for (int colonne = 0 + ligne; colonne < this.taille; colonne++)
                {
                    if (ligne == colonne)
                    {
                        this.tab[ligne, colonne] = 0;
                    }
                    else
                    {
                        this.tab[ligne, colonne] = rand.Next(0, 2);
                        this.tab[colonne, ligne] = this.tab[ligne, colonne];
                    }
                }
            }
        }

        //Affiche la matrice d'adjacence
        public void printMatrice()
        {
            String tmp = "";
            Console.Write("  ");

            for (int i = 0; i < taille; i++)
            {
                Console.Write(this.info[i] + " ");
            }
            Console.WriteLine("\n");

            for (int ligne = 0; ligne < this.taille; ligne++)
            {
                for (int colonne = 0; colonne < this.taille; colonne++)
                {
                    tmp += this.tab[ligne, colonne] + "-";
                }
                tmp = tmp.Substring(0, tmp.Length-1);
                tmp = this.info[ligne] + " " + tmp;
                Console.WriteLine(tmp);
                tmp = "";
            }
        }

        //Supprime le noeud passé en entré
        public void deleteEdge(char Edge)
        {
            int i = 0;
            int j = 0;
            int[,] newtab;
            int pos = this.info.IndexOf(Edge);

            if (pos != -1)
            {
                newtab = new int[this.taille - 1, this.taille - 1];

                if (pos != this.taille - 1)
                {
                    for (int ligne = 0; ligne < this.taille; ligne++)
                    {
                        if (ligne == pos)
                        {
                            ligne++;
                        }

                        for (int colonne = 0; colonne < this.taille; colonne++)
                        {
                            if (colonne == pos)
                            {
                                colonne++;
                            }
                            newtab[i, j] = this.tab[ligne, colonne];
                            j++;
                        }

                        j = 0;
                        i++;
                    }
                }

                // On supprime le dernier élément
                else
                {
                    for (int ligne = 0; ligne < this.taille - 1; ligne++)
                    {
                        for (int colonne = 0; colonne < this.taille - 1; colonne++)
                        {
                            newtab[ligne, colonne] = this.tab[ligne, colonne];
                        }
                    }
                }

                this.info.RemoveAt(pos);
                this.taille = taille - 1;
                this.tab = newtab;
            }

            else { Console.WriteLine("Impossible de retirer le sommet " + Edge + ", il n'existe pas dans la matrice !"); }
        }

        //Ajoute un sommet isolé
        public void addEdge(char Edge)
        {
            int[,] newtab;

            if (this.info.IndexOf(Edge) == -1)
            {
                newtab = new int[taille + 1, taille + 1];

                for (int ligne = 0; ligne < this.taille; ligne++)
                {
                    for (int colonne = 0; colonne < this.taille; colonne++)
                    {
                        newtab[ligne, colonne] = this.tab[ligne, colonne];
                    }
                }

                for (int i = 0; i < this.taille + 1; i++)
                {
                    newtab[this.taille, i] = 0;
                    newtab[i, this.taille] = 0;
                }

                this.taille = this.taille + 1;
                this.tab = newtab;
                this.info.Add(Edge);
            }
            else { Console.WriteLine("Impossible d'ajouter " + Edge + ", un sommet du même nom existe déjà !"); }
        }

        //Modifie le nombre d'arc entre 2 noeuds
        public void setArcValue(char Edge1, char Edge2, int value)
        {
            int pos1 = this.info.IndexOf(Edge1);
            int pos2 = this.info.IndexOf(Edge2);

            if ((pos1 != -1) && (pos2 != -1))
            {
                this.tab[pos1, pos2] = value;
                this.tab[pos2, pos1] = value;
            }

            else { Console.WriteLine("Un des sommets n'existe pas dans la matrice"); }
        }

        //Importe un CSV dans la matrice courante
        public void importCsv(string file)
        {
            int total = 0;
            int nbligne = 0;
            string line;
            string[] currentLine;
            FileStream aFile = new FileStream(file, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);

            while ((line = sr.ReadLine()) != null)
            {
                total++;
            }

            #region relecture fichier
            aFile.Close();
            sr.Close();
            aFile = new FileStream(file, FileMode.Open);
            sr = new StreamReader(aFile);
            #endregion

            this.taille = total;
            this.tab = new int[total, total];
            this.info.Clear();

            while ((line = sr.ReadLine()) != null)
            {
                currentLine = line.Split(';');
                for (int i = 0; i < currentLine.Length; i++)
                {
                    this.tab[nbligne, i] = int.Parse(currentLine[i]);
                }
                this.info.Add((char)(nbligne + 65));
                nbligne++;
            }
            sr.Close();
        }

        public List<List<Char>> Malgrange()
        {
            Random rand = new Random();
            List<List<char>> ListeCFC = new List<List<char>>();
            List<Char> CFC = new List<Char>();
            List<Char> Sommets = new List<char>(this.info);
            List<Char> Successeurs = new List<Char>();
            List<Char> Predecesseurs = new List<Char>();
            int sommet_elu = 0;

            while (Sommets.Count > 0)
            {
                sommet_elu = rand.Next(0, Sommets.Count);
                Successeurs = DFS(this.info.IndexOf(Sommets[sommet_elu]));
                // Trouver les preds...
                Predecesseurs = predecessor(this.info.IndexOf(Sommets[sommet_elu]));
                foreach (char sommet in Successeurs)
                {
                    if (Predecesseurs.Contains(sommet))
                    {
                        CFC.Add(sommet);
                        Sommets.Remove(sommet);
                    }
                }
                ListeCFC.Add(new List<char>(CFC));
                CFC.Clear();
            }

            return ListeCFC;
        }

        public List<char> BFS(int racine)
        {
            List<char> res = new List<char>();

            Queue<int> f = new Queue<int>();
            bool[] marqued = new bool[this.taille];
            for (int i = 0; i < this.taille; i++)
                marqued[i] = false;
            marqued[racine] = true;
            f.Enqueue(racine);
            int x;
            while (f.Count != 0)
            {
                x = f.Dequeue();
                res.Add(this.info[x]);

                for (int i = 0; i < this.taille; i++)
                {
                    if (this.tab[i, x] > 0)
                        if (!marqued[i])
                        {
                            marqued[i] = true;
                            f.Enqueue(i);
                        }
                }
            }

            return res;
        }

        public List<char> DFS(int racine)
        {
            pDFS = new List<char>();
            bool[] explored = new bool[this.taille];
            for (int i = 0; i < this.taille; i++)
                explored[i] = false;
            recDFS(this.tab, racine, this.taille, explored);

            return pDFS;
        }

        public void recDFS(int[,] a, int racine, int n, bool[] explored)
        {
            pDFS.Add(this.info[racine]);
            explored[racine] = true;
            for (int i = 0; i < n; i++)
            {
                if (a[i, racine] > 0)
                    if (!explored[i])
                    {
                        recDFS(a, i, n, explored);
                    }
            }
        }

        
                public List<char> predecessor(int racine)
                {
            
                    pred = new List<char>();
                    bool[] explored = new bool[this.taille];
                    for (int i = 0; i < this.taille; i++)
                    {
                        explored[i] = false;
                    }

                    recPredecessor(this.tab, racine, this.taille, explored);
                    return pred;
                }

                public void recPredecessor(int[,] a, int racine, int n, bool[] explored)
                {
                    pred.Add(this.info[racine]);
                    explored[racine] = true;
                    for (int i = 0; i < n; i++)
                    {
                        if (a[racine, i] > 0)
                        {
                            if (!explored[i])
                            {
                                recDFS(a, i, n, explored);
                            }
                        }
                    }
                }

        public int[,] Warshall()
        {
            int n = this.taille;
            int[,] d = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    d[i, j] = this.tab[i, j];
                }
            }
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        int dik = d[i, k], dkj = d[k, j];
                        if (dik == M || dkj == M) continue;
                        int u = dik + dkj;
                        if (u < d[i, j])
                        {
                            d[i, j] = u;
                        }
                    }
                }
            }
            return d;
        }
	

    }
}
