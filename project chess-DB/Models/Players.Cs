using System.Runtime.InteropServices;

public class Player
{
    //informations obligatoires
    private string username;
    private string mail;
    private string country;
    private int age;
    //informations facultatives (? pour facultatifs)
    private string phone_number;
    private string? name;
    private string? last_name;
    public Player(string username, string mail, string country, int age, string phone_number, string? name = null, string? last_name = null)
    {
        this.username = username;
        this.mail = mail;
        this.country = country;
        this.age = age;
        this.phone_number = phone_number;
        this.name = name;
        this.last_name = last_name;
    }

}