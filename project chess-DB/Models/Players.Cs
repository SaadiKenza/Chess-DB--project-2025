using System.Runtime.InteropServices;

public class Player
{
    //informations obligatoires
    private string username;
    private string mail;
    private string country;
    private int age;
    //informations facultatives (? pour facultatifs)
    private string? phone_number;
    private string? name;
    private string? last_name;
    public Player (string username, string mail, string country, int age)
    {
        this.username = username;
        this.mail = mail;
        this.country = country;
        this.age = age;
    }

}