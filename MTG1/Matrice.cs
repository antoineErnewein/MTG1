using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MTG1
{
	public class Matrice
	{
        public int taille;
        public int[,] tab;
        public List<char> info;
        static List<char> pDFS;
		private string type;
		
		public Matrice(int n,string type)
        {
            this.taille = n;
            this.tab = new int[n, n];
            this.info = new List<char>();
        	this.type = type;
		}
		public void fillMatrice()
        {
		        Random rand = new Random();
		
		        for (int i = 0; i < this.taille; i++)
		        {
		            this.info.Add((char)(i + 65));
		        }
		
				if(this.type == "NO")
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
				else if (this.type == "O")
				for (int ligne = 0; ligne < this.taille; ligne++)
		        {
		            for (int colonne = 0 ; colonne < this.taille; colonne++)
		            {
		            	        this.tab[ligne, colonne] = rand.Next(0, 2);
					}
		        }
				else if (this.type == "OP")
				for (int ligne = 0; ligne < this.taille; ligne++)
		        {
		            for (int colonne = 0 ; colonne < this.taille; colonne++)
		            {
		            	        if(rand.Next(0, 2) == 1)
									this.tab[ligne, colonne] = rand.Next(0,10);
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
		 //Parcours d'arbre en largeur
        public List<char> BFS(int racine)
        {
            int nb_operations = 0;
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
                    nb_operations++;
                    if (this.tab[i, x] > 0)
                        if (!marqued[i])
                        {
                            nb_operations++;
                            marqued[i] = true;
                            f.Enqueue(i);
                        }
                }
            }
            Console.WriteLine("\nNombre d'opérations effectuées : " + nb_operations + "\n");
            return res;
        }
		
		 //Donne la composante fortement connexe du graphe
        public List<List<Char>> Malgrange()
        {
            Integer nb_operations = new Integer(0);
            Random rand = new Random();
            List<List<char>> ListeCFC = new List<List<char>>();
            List<Char> CFC = new List<Char>();
            List<Char> Sommets = new List<char>(this.info);
            List<Char> Successeurs = new List<Char>();
            List<Char> Predecesseurs = new List<Char>();
            int sommet_elu = 0;

            //Trouver condition pour sommet isolé (non pris en charge par DFS)
            while (Sommets.Count > 0)
            {
                sommet_elu = rand.Next(0, Sommets.Count);
                Successeurs = DFS(this.info.IndexOf(Sommets[sommet_elu]), nb_operations, false);
                Predecesseurs = predecesseurs(this.info.IndexOf(Sommets[sommet_elu]), nb_operations);
                foreach (char sommet in Successeurs)
                {
                    if (Predecesseurs.Contains(sommet))
                    {
                        nb_operations.nombre = nb_operations.nombre + Predecesseurs.Count; //On compare chaque élément dans les prédécesseurs
                        CFC.Add(sommet);
                        Sommets.Remove(sommet);
                    }
                }
                //Cas sommet isolé
                /*if (Successeurs.Count == 0 && Predecesseurs.Count == 0)
                {
                    CFC.Add(Sommets[Sommets]);
                    Sommets.Remove(sommet);
                }*/
                ListeCFC.Add(new List<char>(CFC));
                CFC.Clear();
            }

            Console.WriteLine("\nNombre d'opérations effectuées : " + nb_operations.nombre + "\n");
            return ListeCFC;
        }
       
	
	    //Parcours d'abre en profondeur 
	    public List<char> DFS(int racine, Integer nb_operations, bool justDFS)
	    {
	        pDFS = new List<char>();
	        bool[] explored = new bool[this.taille];
	        for (int i = 0; i < this.taille; i++)
	            explored[i] = false;
	        recDFS(this.tab, racine, this.taille, explored, nb_operations);

            if (justDFS)
            {
                Console.WriteLine("\nNombre d'opérations effectuées : " + nb_operations.nombre + "\n");
            }
	        return pDFS;
	    }
	
	    //Procédure récursive utilisée dans DSF
	    public void recDFS(int[,] a, int racine, int n, bool[] explored, Integer nb_operations)
	    {
	        pDFS.Add(this.info[racine]);
	        explored[racine] = true;
	        for (int i = 0; i < n; i++)
	        {
                nb_operations.nombre++;
	            if (a[i, racine] > 0)
	                if (!explored[i])
	                {
                        nb_operations.nombre++;
	                    recDFS(a, i, n, explored, nb_operations);
	                }
	        }
	    }
	
	            //Cherche les prédécesseurs d'un sommet
	            public List<char> predecesseurs(int racine, Integer nb_operations)
	            {
                List<Char> predecesseurs = new List<char>();
                int indicepred = 0;
                bool[] explored = new bool[this.taille];
                for (int i = 0; i < this.taille; i++)
                {
                    explored[i] = false;
                }

                for (int i = 0; i < this.taille; i++)
                {
                    nb_operations.nombre++;
                    if (this.tab[racine, i] > 0)
                    {
                        if (!explored[i])
                        {
                            nb_operations.nombre++;
                            predecesseurs.Add(this.info[i]);
                        }
                    }
                }

                while(prochainPred(predecesseurs, explored, nb_operations) != -1)
                {
                    nb_operations.nombre++;
                    indicepred = prochainPred(predecesseurs, explored, nb_operations);
                    explored[indicepred] = true;

                    for (int i = 0; i < this.taille; i++)
                    {
                        nb_operations.nombre++;
                        if (this.tab[indicepred, i] > 0)
                        {
                            if (!explored[i])
                            {
                                nb_operations.nombre++;
                                predecesseurs.Add(this.info[i]);
                            }
                        }
                    }

                }

                return predecesseurs;
            }

            //Cherche le prochain prédecesseur
            public int prochainPred(List<char> pred, bool[] explored, Integer nb_operations)
            {
                int res = -1;

                for (int i = 0; i < pred.Count; i++)
                {
                    nb_operations.nombre++;
                    if (!explored[this.info.IndexOf(pred[i])])
                    { 
                        res = this.info.IndexOf(pred[i]);
                        break;
                    }
                }

                return res;
            }

            //Calcul la fermeture transitive
            public int[,] Warshall()
            {
                int nb_operations = 0;

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
                            nb_operations++;
                            if (d[i, k] + d[k, j] == 2 && i != j)
                            {
                                d[i, j] = 1;
                            }
                        }
                    }
                }
                Console.WriteLine("\nNombre d'opérations effectuées : " + nb_operations + "\n");
                return d;
            }
		public int nbSommets()
		{
			return this.taille;	
		}

        public int maxDegree()
        {
            int nb_operations = 0;
            int maxdegree = 0;
            int degreCourant = 0;

            for (int i = 0; i < this.taille; i++)
            {
                degreCourant = 0;
                for (int j = 0; j < this.taille; j++)
                {
                    nb_operations++;
                    if (this.tab[i, j] > 0)
                    degreCourant++;
                }

                nb_operations++;
                if (degreCourant > maxdegree)
                {
                    maxdegree = degreCourant;
                }
            }
            Console.WriteLine("\nNombre d'opérations effectuées : " + nb_operations + "\n");
            return maxdegree;
        }

        public int minDegree()
        {
            int nb_operations = 0;
            int mindegree = int.MaxValue;
            int degreCourant = 0;

            for (int i = 0; i < this.taille; i++)
            {
                degreCourant = 0;
                for (int j = 0; j < this.taille; j++)
                {
                    nb_operations++;
                    if(this.tab[i, j] > 0)
                    degreCourant++;
                }

                nb_operations++;
                if (degreCourant < mindegree)
                {
                    mindegree = degreCourant;
                }
            }
            Console.WriteLine("\nNombre d'opérations effectuées : " + nb_operations + "\n");
            return mindegree;
        }
    }
}

