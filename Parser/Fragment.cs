using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;

public class CFragment
{
    public CFragment parent;
    public List<CFragment> children = new List<CFragment>();
    public enum Typ {PLIK, BLOK_KOMENTARZY, KOMENTARZ, PROCEDURA, FRAGMENT_PROCEDURY, KLUCZOWE, INNE,NIEZNANE};
    public Typ typ = Typ.INNE;
    public string text;
 
    public CFragment()
	{
	}

    public CFragment(CFragment rodzic)
    {
        parent = rodzic;
    }

    public void RemoveComment()
    {
        string[] argument = new string[] { "//", "\r\n" };
    }

    string AddLine(string stary,string nowy)
    {
        if (stary == "") return nowy;
        if (nowy == "") return stary;
        return (stary + "\r\n" + nowy);
    }
        
    public CFragment Nowy(string wartosc, Typ typ)
    {
        if (typ == Typ.INNE)
        {
            if(wartosc!="") Nowy(wartosc).typ = Typ.INNE;
            //dodaj szukanie procedur
        } else
        if (typ == Typ.NIEZNANE)
        {//szukamy komentarzy
            //TODO wyrzuc to do osobnej funkcji- ma byc porzadek
            //rozbijamy na linijki
            string[] komentarz = new string[] { "//" };
            string[] nowa_linia = new string[] { "\r\n" };
            string[] result = wartosc.Split(nowa_linia, System.StringSplitOptions.None);
            string temp = "";
            foreach(string line in result)
            {//wyluskujemy komentarze
                string[] test = line.Split(komentarz,2, System.StringSplitOptions.None);
                //sklejamy linijki
                temp = AddLine(temp, test[0]);
                if (test.Count() > 1)
                {//jest komentarz w tej linii                    
                    if (test[0]=="")
                    {//jezeli komentarz byl od nowej linii to dodaj go jako zwyklego potomka
                        Nowy(temp, Typ.INNE);
                        Nowy(test[1]).typ = Typ.KOMENTARZ;
                    } else
                    {//jezeli w tej samej linni to jest jej potomkiem
                        Nowy(temp, Typ.INNE).Nowy(test[1]).typ = Typ.KOMENTARZ;
                    }
                    temp = "";
                }
            }
            Nowy(temp, Typ.INNE);
        }
        return children[children.Count - 1];
    }

    public CFragment(string wartosc)
    {
        string[] result;
        string[] argument = new string[] { "/*", "*/" };
        int counter = 0;

        typ = Typ.PLIK;
        //[0] istnieje zawsze i zawiera to co przed separatorem
        //[1] istnieje tylko gdy jest separator
        result = wartosc.Split(argument, System.StringSplitOptions.None);
        foreach (string fragment in result)
        {
            if (counter % 2 == 0)
            {
                if (fragment != "") Nowy(fragment,Typ.NIEZNANE);
            }
            else
            {
                if (fragment != "") Nowy(fragment).typ = Typ.BLOK_KOMENTARZY;
            }
            counter++;
        }
    }

    private void RemoveBlockComment(string wartosc)
    {

    }

    private void RemoveComment(string wartosc)
    {

    }

    public CFragment Nowy()
    {
        children.Add(new CFragment(this));
        return children[children.Count-1];
    }

    public CFragment Nowy(string wartosc)
    {
        children.Add(new CFragment(this));
        children[children.Count - 1].text = wartosc;
        return children[children.Count - 1];
    }

    public void Print()
    {

    }

    public void PrintAll(TextBox editBox)
    {
        editBox.Text += text + "\r\n";
        children.ForEach(delegate (CFragment element) 
        {
            element.PrintAll(editBox);
        });
    }
}
