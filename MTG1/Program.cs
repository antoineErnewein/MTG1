﻿using System;
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
            Matrice mat = new Matrice(1,"");
            bool again = true;
            Console.WriteLine(" -----------------------------\n| G-ToolBox V3.0 - (08/12/12) |\n -----------------------------\n");
            Console.WriteLine("1) Creer une matrice aleatoire a N sommets (Non orienté)\n" +
            	"2) Creer une matrice aleatoire a N sommets (orienté)\n" +
            	"3) Creer une matrice aleatoire a N sommets (oriénté + pondéré)" +
            	"\n4) Importer un fichier CSV\n");
            int choix = int.Parse(Console.ReadLine());
			int n;
            switch (choix)
            {
                case 1 :
                    Console.WriteLine("Nombre de sommets souhaité :");
                    n = int.Parse(Console.ReadLine());
                    mat = new Matrice(n,"NO");
                    mat.fillMatrice();
                    break;
				case 2 :
                    Console.WriteLine("Nombre de sommets souhaité :");
                    n = int.Parse(Console.ReadLine());
                    mat = new Matrice(n,"O");
                    mat.fillMatrice();
                    break;
				case 3 :
                    Console.WriteLine("Nombre de sommets souhaité :");
                    n = int.Parse(Console.ReadLine());
                    mat = new Matrice(n,"OP");
                    mat.fillMatrice();
                    break;
                case 4 :
                    Console.WriteLine("Chemin du fichier CSV :");
                    string chemin = Console.ReadLine();
                    mat = new Matrice(1,"");
                    mat.importCsv(chemin);
                    break;
            }


            Console.Clear();

            while (again)
            {
                Console.WriteLine(" -----------------------------\n| G-ToolBox V3.0 - (08/12/12) |\n -----------------------------\n");
                Console.WriteLine("Matrice d'adjacence :\n\n");
                mat.printMatrice();
				String menu = "\nActions :\n"+
					"1) Ajouter un noeud\n2) Supprimer un noeud\n" +
                    "3) Modifier le nombre d'arc entre deux noeuds\n4) Afficher le nombre de sommets\n5) Afficher le degré minimum\n6) Afficher le degré maximum\n" +
					"10) Parcours BFS\n" +
					"11) Parcours DFS\n12) CFC selon Malgrange\n13) Warshall\n0) Quitter";
                Console.WriteLine(menu);
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
					case 4 :
						Console.Write("Nombre de sommets : "+mat.nbSommets());
						Console.ReadLine();
						break;
                    case 5:
                        Console.Write("Degré minimum : " + mat.minDegree());
                        Console.ReadLine();
                        break;
                    case 6:
                        Console.Write("Degré maximum : " + mat.maxDegree());
                        Console.ReadLine();
                        break;
                    case 10:
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
                    case 11:
                        Console.WriteLine("\nSommet racine : ");
                        List<char> pDFS = mat.DFS(mat.info.IndexOf(Console.ReadLine()[0]),new Integer(0), true);
                        string parcours2 = "[ ";
                        foreach (char c in pDFS)
                        {
                            parcours2 += c + ", ";
                        }
                        parcours2 = parcours2.Substring(0, parcours2.Length - 2) + " ]";
                        Console.WriteLine(parcours2);
                        Console.ReadLine();
                        break;

                    case 12:
                        Console.WriteLine("Composante Fortement connexe du graphe :");
                        List<List<char>> CFC = mat.Malgrange();
                        string cfc = "";
                        foreach (List<char> l in CFC)
                        {
                            cfc += "<";
                            foreach(char c in l)
                            {
                            cfc += c + ", ";
                            }
                            cfc += ">";
                        }

                        Console.WriteLine(cfc);
                        Console.ReadLine();
                        break;

                    case 13:
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

                    case 0 :
                        again = false;
                        break;
                }
                Console.Clear();
            }
            
        }
        //Remplit aléatoirement une matrice (sans boucle sur les sommets)
        
		/*
      */
	}
}
