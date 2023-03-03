class Program
{
    public static void Main()
    {
        Game game = new Game();
        game.Play();
    }
}

public class Player
{
    private string _PlayerName;
    private int _Point;
    private string _Forme;

    public Player(string PlayerName)
    {
        this._PlayerName = PlayerName;
        this._Point = 0;
    }

    public string PlayerName
    {
        get { return this._PlayerName; }    
    }

    public string Forme
    {
        get { return this._Forme; }
        set { this._Forme = value; }
    }

    public int Point
    {
        get { return this._Point; }
        set { this._Point += value; }
    }

}

public class Game
{
    private Grille _Grille;
    private Player[] _Players;
    private int _CurrencyPlayer;
    private int _NextPlayer;
    public Game()
    {
        this._Grille = new Grille();
        this._Players = new Player[2];
        this._Players[0] = InitalisePlayer(1);
        this._Players[1] = InitalisePlayer(2);
        SelectPlayerStart();
        SelectForm();
    }

    public void Play()
    {
        this._Grille.ShowGrille();
        do
        {
            AddForme();
            AnalyseGrille();
            Console.WriteLine();
        } while (this._Players[this._NextPlayer].Point < 3);
        this._Grille.ShowGrille();
        Console.WriteLine("{0} tu as gagné la partie", this._Players[this._NextPlayer].PlayerName);
        Console.WriteLine("Score final: {0}: {1} - {2}: {3}", this._Players[this._CurrencyPlayer].PlayerName, this._Players[this._CurrencyPlayer].Point, this._Players[this._NextPlayer].PlayerName, this._Players[this._NextPlayer].Point);
    }

    private void AnalyseGrille()
    {
        this._Grille.ShowGrille();
        if (this._Grille.End == true)
        {
            FinishGame();
        }
        else if (this._Grille.Complet == true)
        {
            CompletGame();
        }
        else
        {
            ChangePlayer();
        }
    }

    private void ChangePlayer()
    {
        int x = this._CurrencyPlayer;
        this._CurrencyPlayer = this._NextPlayer;
        this._NextPlayer = x;
    }

    private void CompletGame()
    {
        Console.WriteLine("La grille est complète, match nul!");
        SelectPlayerStart();
        this._Grille.Initialise();
        this._Grille.End = false;
        this._Grille.Complet = false;
    }

    private void FinishGame()
    {
        this._Players[this._CurrencyPlayer].Point = 1;
        Console.WriteLine("Félicitations {0}, il ne te reste que {1} point(s) à marquer pour gagner la partie", this._Players[this._CurrencyPlayer].PlayerName, 3 - this._Players[this._CurrencyPlayer].Point);
        Console.WriteLine("Score actuelle: {0}: {1} point(s) - {2}: {3} point(s)", this._Players[this._CurrencyPlayer].PlayerName, this._Players[this._CurrencyPlayer].Point, this._Players[this._NextPlayer].PlayerName, this._Players[this._NextPlayer].Point);
        SelectPlayerStart();
        this._Grille.Initialise();
        this._Grille.End = false;
        this._Grille.Complet = false;
    }

    private Player InitalisePlayer(int playerNumber)
    {
        Console.WriteLine("Joueur {0} entrez votre pseudo", playerNumber);
        string pseudo = Console.ReadLine();
        Player Player = new Player(pseudo);
        return Player;
    }

    private void AddForme()
    {
        Console.WriteLine("C'est au tour de {0}, tu joues avec les '{1}'", this._Players[this._CurrencyPlayer].PlayerName, this._Players[this._CurrencyPlayer].Forme);
        bool correct = false;
        string y;
        string x;
        correct = StateCoordonne(correct, out y, out x);
        this._Grille.ChangeGrille(int.Parse(x) - 1, int.Parse(y) - 1, this._Players[this._CurrencyPlayer].Forme);
    }

    private bool StateCoordonne(bool correct, out string y, out string x)
    {
        do
        {
            x = GetX();
            y = GetY();
            if (this._Grille.GrilleRead(int.Parse(x) - 1, int.Parse(y) - 1) == ".")
            {
                correct = true;
            }
            else
            {
                Console.WriteLine("Cette case a déja été joué dans cette partie");
            }
        } while (correct != true);
        return correct;
    }

    private string GetY()
    {
        string y;
        do
        {
            Console.WriteLine("Tape la colonne (1,2 ou 3) ou tu souhaites poser '{0}' dans la grille", this._Players[this._CurrencyPlayer].Forme);
            y = Console.ReadLine();
        } while (y != "1" && y != "2" && y != "3");
        return y;
    }

    private string GetX()
    {
        string x;
        do
        {
            Console.WriteLine("Tape la ligne (1,2 ou 3) ou tu souhaites poser '{0}' dans la grille", this._Players[this._CurrencyPlayer].Forme);
            x = Console.ReadLine();
        } while (x != "1" && x != "2" && x != "3");
        return x;
    }

    private void SelectPlayerStart()
    {
        Random rand = new Random();
        this._CurrencyPlayer = rand.Next(0,2);
        do
        {
            this._NextPlayer = rand.Next(0,2);
        } while (this._CurrencyPlayer == this._NextPlayer);
    }

    private void SelectForm()
    {
        string forme;
        do
        {
            Console.WriteLine("{0}, tu es le 1er joueur à commencer. Tape 'X' pour les sélectionner ou 'O'", this._Players[this._CurrencyPlayer].PlayerName);
            forme = Console.ReadLine();
        } while (forme != "X" && forme != "O");
        if (forme == "X")
        {
            this._Players[this._CurrencyPlayer].Forme = "X";
            this._Players[this._NextPlayer].Forme = "O";
        }
        else
        {
            this._Players[this._CurrencyPlayer].Forme = "O";
            this._Players[this._NextPlayer].Forme = "X";
        }
    }
}

public class Grille
{
    private string[,] _Grille;
    private bool _End;
    private bool _Complet;
    private int _CoupsRestant;
    public Grille()
    {
        Initialise();
        this._End = false;
        this._Complet = false;
    }

    public bool Complet
    {
        get { return this._Complet; }
        set { this._Complet = value; }
    }

    public string GrilleRead(int x, int y)
    {
        return this._Grille[x, y];
    }

    private void GrilleComplet()
    {
        for(int i = 0; i < this._Grille.GetLength(0); i++)
        {
            for(int j = 0; j < this._Grille.GetLength(1); j++)
            {
                if(this._Grille[i, j] == ".")
                {
                    this._Complet = false;
                    break;
                }
                else
                {
                    this._Complet = true;
                }
            }
        }
    }

    public bool End
    {
        get { return this._End; }
        set { this._End = value; }
    }

    public void ChangeGrille(int x, int y, string forme)
    {
        this._Grille[x, y] = forme;
        VerifGrille();
    }

    private void VerifGrille()
    {
        VerificationX();
        VerificationY();
        VerificationDiagonale();
        GrilleComplet();
    }

    private void VerificationDiagonale()
    {
        if ((this._Grille[0, 0] == this._Grille[1, 1] && this._Grille[2, 2] == this._Grille[1, 1]) && (this._Grille[1,1] != "."))
        {
            this._End = true;
        }
        if ((this._Grille[0, 2] == this._Grille[1, 1] && this._Grille[2, 0] == this._Grille[1, 1]) && (this._Grille[1, 1] != "."))
        {
            this._End = true;
        }
    }

    private void VerificationY()
    {
        for (int i = 0; i < this._Grille.GetLength(0); i++)
        {
            if ((this._Grille[0, i] == this._Grille[1, i] && this._Grille[2, i] == this._Grille[1, i]) && (this._Grille[1, i] != "."))
            {
                this._End = true;
            }
        }
    }

    private void VerificationX()
    {
        for (int i = 0; i < this._Grille.GetLength(0); i++)
        {
            if (this._Grille[i, 0] == this._Grille[i, 1] && this._Grille[i, 2] == this._Grille[i, 1] && (this._Grille[i,1] != "."))
            {
                this._End = true;
            }
        }
    }

    public void Initialise()
    {
        string[,] morpion = new string[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                morpion[i, j] = ".";
            }
        }
        this._Grille = morpion;
    }


    public void ShowGrille()
    {
        Console.WriteLine("-----");
        Console.WriteLine("{0}|{1}|{2}", this._Grille[0, 0], this._Grille[0, 1], this._Grille[0, 2]);
        Console.WriteLine("-----");
        Console.WriteLine("{0}|{1}|{2}", this._Grille[1, 0], this._Grille[1, 1], this._Grille[1, 2]);
        Console.WriteLine("-----");
        Console.WriteLine("{0}|{1}|{2}", this._Grille[2, 0], this._Grille[2, 1], this._Grille[2, 2]);
        Console.WriteLine("-----");
    }
}