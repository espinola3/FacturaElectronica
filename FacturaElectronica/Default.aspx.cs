using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;


namespace FacturaElectronica
{
    public partial class Default : System.Web.UI.Page
    {
        string a3, a4, a5, a6, a8, IM2;
        string WebUserID, ResellerID, BrCustName, FirstName, LastName;
        string webUserMail,AdminMail,FacturaMail,Pending,FacturaMailUpdate;
        string MailCC, MailBCC, MailTo, MailFrom, MailSubject, MailBody;
        SqlConnection conn = new SqlConnection();
        



        protected void Page_Load(object sender, EventArgs e)
        {
            conn.ConnectionString = "Data Source=10.131.16.132; Initial Catalog=IMONLINE; User Id =user_ecomm_write;Password=2bR!S2rqXq";

            if (IsPostBack == false)
            {
                try
                {
                    a3 = Request.QueryString["a3"];
                    a4 = Request.QueryString["a4"];
                    a5 = Request.QueryString["a5"];
                    a6 = Request.QueryString["a6"];
                    a8 = Request.QueryString["a8"];
                    IM2 = Request.QueryString["IM2"];
                }
                catch
                {

                }

                try
                {
                    DecoderEndeavour.Class1 Decoder = new DecoderEndeavour.Class1();
                    WebUserID = Decoder.Decrypt(IM2);
                    ResellerID = Decoder.Decrypt(a3);
                    FirstName = Decoder.Decrypt(a4);
                    LastName = Decoder.Decrypt(a5);
                    webUserMail = Decoder.Decrypt(a6);
                    BrCustName = Decoder.Decrypt(a8);


                    //WebUserID = "{3193c4ac-e9b8-42bb-b8ca-428f44d69831}";
                    //ResellerID = "29107187";
                    //FirstName = "Jordi";
                    //LastName = "Carbó";
                    //webUserMail = "jcarbo@ingrammicro.es";
                    //BrCustName = "INGRAM MICRO TEST CUSTOMER";

                    Page.Session["WebUserID"] = WebUserID;
                    Page.Session["ResellerID"] = ResellerID;
                    Page.Session["FirstName"] = FirstName;
                    Page.Session["LastName"] = LastName;
                    Page.Session["webUserMail"] = webUserMail;
                    Page.Session["BrCustName"] = BrCustName;




                    consulta_Estat_Client(ResellerID.Remove(0, 2));
                    consulta_Drets_Usuari(WebUserID, ResellerID);

                    Label1.Text = BrCustName;
                    Label2.Text = ResellerID;

                }
                catch
                {
                    MailBody = "Error x003 - Error al decodificar els parametres:\na3: " + a3 + "\na4: " + a4 + "\na5: " + a5 + "\na6: " + a8 + "\na8: " + a8 + "\nIM2: " + IM2;
                    SendMail("Error Factura Electronica", MailBody, "comunicacion@ingrammicro.es", "ernest.espinola@ingrammicro.com", "", "");
                }
            }
            else
            {
                WebUserID=Convert.ToString(Page.Session["WebUserID"]);
                ResellerID=Convert.ToString(Page.Session["ResellerID"]);
                FirstName=Convert.ToString(Page.Session["FirstName"]);
                LastName=Convert.ToString(Page.Session["LastName"]);
                webUserMail = Convert.ToString(Page.Session["webUserMail"]);
                BrCustName=Convert.ToString(Page.Session["BrCustName"]);
                FacturaMail = Convert.ToString(Page.Session["FacturaMail"]);
                Pending = Convert.ToString(Page.Session["Pending"]);
            }

            

            

            
        }

        protected void B_enviar_Click(object sender, EventArgs e)
        {

            FacturaMailUpdate = T_email.Text;
            string queryInsert = "INSERT INTO Alta_Factura_Electronica (Br,CustNbr,CustName,DestinationType,Email1,Email2,Pending) VALUES ('" + ResellerID.Remove(0, 2) + "','" + ResellerID.Remove(2, 6)+"','"+BrCustName+"','3','"+T_email.Text+"','imesfacturacionelectronica@ingrammicro.com','1')" ;
            string queryUpdate = "UPDATE Alta_Factura_Electronica SET  Email1='" + FacturaMailUpdate + "',Email2='imesfacturacionelectronica@ingrammicro.com',Pending='1' where CustNbr='" + ResellerID.Remove(0, 2)+"'";

            if ((Pending == "0") || (Pending == "1") || (Pending == "2")) //UPDATE DEL MAIL PERQUÈ JA ESTÀ A LA TAULA DE IMONLINE
            {
                try
                {
                    MailSubject="";
                    conn.Open();
                    SqlCommand selectPending = new SqlCommand(queryUpdate, conn);
                    selectPending.ExecuteNonQuery();
                    conn.Close();
                    SendMail(MailSubject, MailBody, "comunicacion@ingrammicro.es", "ernest.espinola@ingrammicro.com", "", "");
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                    T_email.Visible = false;
                    B_enviar.Visible = false;
                    Label3.Text = "Error x001. Contacta con el siguiente correo para más información:<br/>imessoporteweb@ingrammicro.com ";
                    MailBody = "Error x001 - Error al fer l'UPDATE al client "+ResellerID+" amb usuari web: "+ WebUserID+"\n\nError: "+ex;
                    SendMail("Error Factura Electronica", MailBody, "comunicacion@ingrammicro.es", "ernest.espinola@ingrammicro.com", "", "");

                }
            }
            else //INSERT DE TOTES LES DADES, PERQUÈ NO ESTÀ DONAT D'ALTA A LA FACTURA ELECTRONICA
            {
                try
                {
                    conn.Open();
                    SqlCommand selectPending = new SqlCommand(queryInsert, conn);
                    selectPending.ExecuteNonQuery();
                    conn.Close();
                    MailTo = webUserMail + ";" + T_email.Text;
                    SendMail(MailSubject, MailBody, "comunicacion@ingrammicro.es", MailTo , "", "");
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                    T_email.Visible = false;
                    B_enviar.Visible = false;
                    Label3.Text = "Error x002. Contacta con el siguiente correo para más información:<br/>imessoporteweb@ingrammicro.com ";
                    MailBody = "Error x002 - Error al fer l'INSERT al client " + ResellerID + " amb usuari web: " + WebUserID + "\n\nError: " + ex;
                    SendMail("Error Factura Electronica", MailBody, "comunicacion@ingrammicro.es", "ernest.espinola@ingrammicro.com", "", "");
                }
            }

            Response.Redirect("HTMLPage1.htm");  

        }
        protected void consulta_Drets_Usuari(string WebUserID,string ResellerID)
        {
            string admin, compras, FirstName, LastName, UserName, AdminMail;
            admin = "";
            compras = "";
            UserName = "";
            AdminMail = "";
            FirstName = "";
            LastName = "";

            string queryDrets = "SELECT * FROM Accesos_IMOnline WHERE UserID='" + WebUserID + "'";
           
            try
            {
                conn.Open();
                SqlCommand selectPending = new SqlCommand(queryDrets, conn);
                SqlDataReader reader1 = selectPending.ExecuteReader();
                reader1.Read();
                admin = reader1["ResellerAdmin"].ToString();
                compras = reader1["PlaceOrders"].ToString();
                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }

            string queryAdmin = "SELECT TOP 1 FROM Accesos_IMOnline WHERE ResellerID='" + ResellerID + "' AND NotificationEmail not in ('jcarbo@ingrammicro.es','jordi.carbo@ingrammicro.com','imessoporteweb@ingrammicro.com') AND ResellerAdmin='Y'";
            try
            {
                conn.Open();
                SqlCommand selectPending = new SqlCommand(queryDrets, conn);
                SqlDataReader reader2 = selectPending.ExecuteReader();
                reader2.Read();
                FirstName = reader2["FirstName"].ToString();
                LastName = reader2["LastName"].ToString();
                AdminMail = reader2["NotificationEmail"].ToString();
                UserName = reader2["Username"].ToString();
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }


            
            if ((admin == "Y") || (compras == "Y"))
            {
                //Ho deixa tot igual
            }
            else 
            {
                T_email.Visible = false;
                B_enviar.Visible = false;
                if (Convert.ToInt32(Pending) == -1)
                {
                    Label3.Text = "No tienes derechos para poder dar de alta a tu empresa en el servicio.<br/>Contacta con " + FirstName + " " + LastName + ":<br/>Username: " + UserName + "<br/>correo: "+AdminMail;
                }
                else
                {
                    Label3.Text = "No tienes derechos para poder modificar<br/>Contacta con " + FirstName + " " + LastName + ":<br/>Username: " + UserName + "<br/>correo: " + AdminMail;
                }
            }
        }
        protected void consulta_Estat_Client(string ResellerID)
        {
            string queryEstat = "SELECT * FROM Alta_Factura_Electronica WHERE CustNbr='" + ResellerID + "'";
            
            try
            {
                conn.Open();
                SqlCommand selectPending = new SqlCommand(queryEstat, conn);
                SqlDataReader reader1 = selectPending.ExecuteReader();
                reader1.Read();
                FacturaMail = reader1["Email1"].ToString();
                Pending = reader1["Pending"].ToString();
                conn.Close();
                Page.Session["Pending"] = Pending;
                Page.Session["FacturaMail"] = FacturaMail;

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }

            switch (Convert.ToInt32(Pending))
            {
                case 0:
                    Label3.Text = "Ya está dado de alta en el servicio de factura electrónica con el siguiente correo. Puede modificarlo cuando desee desde aquí.";
                    T_email.Text = FacturaMail;
                    break;
                case 1:
                    Label3.Text = "Se está tramitando el cambio de correo en el servicio de factura electronica. Puede modificarlo cuando desee desde aquí.";
                    T_email.Text = FacturaMail;
                    break;
                case 2:
                    Label3.Text = "Se está tramitando el alta en el servicio de factura electrónica con el siguiente correo. Puede modificarlo cuando desee desde aquí.";
                    T_email.Text = FacturaMail;
                    break;
                default:
                    Label3.Text = "Puede introducir el correo donde quiere recibir las facturas en el siguiente campo.";
                    break;

            }
        }

        protected void SendMail(string Subject,string Body,string From,string To,string CC,string Bcc)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                //Especificamos el correo desde el que se enviará el Email
                mail.From = new MailAddress(From);
                //Aquí ponemos el asunto del correo
                mail.Subject = Subject;
                //Aquí ponemos el mensaje que incluirá el correo
                mail.Body = Body;
                //Especificamos a quien enviaremos el Email
                mail.To.Add(new MailAddress(To));
                if(CC!="")
                mail.CC.Add(new MailAddress(CC));
                if (Bcc != "")
                mail.Bcc.Add(new MailAddress(Bcc));

                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient();
                //smtpclient.Host = "localhost"; //-------this has to given the Mailserver IP
                smtpclient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                smtpclient.Host = "172.31.16.50";
                //smtpclient.EnableSsl = true;
                smtpclient.Send(mail);


                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
//Errors web:
//x001: Error al fer l' UPDATE al clickar el botó 
//x002: Error al fer l' INSERT al clickar el botó 
//x003: Error al decodificar els parametres 