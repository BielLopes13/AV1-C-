using System;

public enum TiposDeArquivo
{
    MP3,
    MP4,
    PDF,
    JPG,
    PNG
}

public interface ICanal
{
    void EnviarMensagem(string destinatario, Mensagem mensagem);
}

public static class Factory
{
    public static ICanal CriarCanal(string canal)
    {
        switch (canal.ToLower())
        {
            case "whatsapp":
                return new WhatsApp();
            case "telegram":
                return new Telegram();
            case "facebook":
                return new Facebook();
            case "instagram":
                return new Instagram();
            default:
                throw new ArgumentException($"Canal '{canal}' não reconhecido.");
        }
    }
}

public class Mensagem
{
    public string Conteudo { get; set; }
    public DateTime DataEnvio { get; set; }
}

public class MensagemTexto : Mensagem
{
    // Pode adicionar propriedades específicas para MensagemTexto se necessário
}

public class MensagemMultimidia : Mensagem
{
    public string Arquivo { get; set; }
    public TiposDeArquivo Formato { get; set; }
}

public class MensagemVideo : MensagemMultimidia
{
    public int Duracao { get; set; }
}

public class WhatsApp : ICanal
{
    public void EnviarMensagem(string destinatario, Mensagem mensagem)
    {
        Console.WriteLine($"Enviando mensagem via WhatsApp para {destinatario}");
        Console.WriteLine($"Conteúdo: {mensagem.Conteudo}");
        Console.WriteLine($"Data de Envio: {mensagem.DataEnvio}");
    }
}

public class Telegram : ICanal
{
    public void EnviarMensagem(string destinatario, Mensagem mensagem)
    {
        Console.WriteLine($"Enviando mensagem via Telegram para {destinatario}");
        Console.WriteLine($"Conteúdo: {mensagem.Conteudo}");
        Console.WriteLine($"Data de Envio: {mensagem.DataEnvio}");
    }
}

public class Facebook : ICanal
{
    public void EnviarMensagem(string destinatario, Mensagem mensagem)
    {
        Console.WriteLine($"Enviando mensagem via Facebook para {destinatario}");
        Console.WriteLine($"Conteúdo: {mensagem.Conteudo}");
        Console.WriteLine($"Data de Envio: {mensagem.DataEnvio}");
    }
}

public class Instagram : ICanal
{
    public void EnviarMensagem(string destinatario, Mensagem mensagem)
    {
        Console.WriteLine($"Enviando mensagem via Instagram para {destinatario}");
        Console.WriteLine($"Conteúdo: {mensagem.Conteudo}");
        Console.WriteLine($"Data de Envio: {mensagem.DataEnvio}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Escolha o canal de comunicação:");
        Console.WriteLine("1. WhatsApp");
        Console.WriteLine("2. Telegram");
        Console.WriteLine("3. Facebook");
        Console.WriteLine("4. Instagram");

        if (!int.TryParse(Console.ReadLine(), out int escolhaCanal) || escolhaCanal < 1 || escolhaCanal > 4)
        {
            Console.WriteLine("Escolha de canal inválida.");
            return;
        }

        Console.Write("Digite o destinatário: ");
        string destinatario = Console.ReadLine();

        Console.Write("Digite a mensagem: ");
        string conteudo = Console.ReadLine();

        Console.WriteLine("Escolha o tipo de mensagem:");
        Console.WriteLine("1. Texto");
        Console.WriteLine("2. Vídeo");

        if (!int.TryParse(Console.ReadLine(), out int escolhaTipoMensagem) || escolhaTipoMensagem < 1 || escolhaTipoMensagem > 2)
        {
            Console.WriteLine("Escolha de tipo de mensagem inválida.");
            return;
        }

        Mensagem mensagem;

        switch (escolhaTipoMensagem)
        {
            case 1:
                mensagem = new MensagemTexto { Conteudo = conteudo, DataEnvio = DateTime.Now };
                break;
            case 2:
                mensagem = CriarMensagemVideo(conteudo);
                break;
            default:
                Console.WriteLine("Tipo de mensagem inválido. Enviando mensagem de texto por padrão.");
                mensagem = new MensagemTexto { Conteudo = conteudo, DataEnvio = DateTime.Now };
                break;
        }

        if (mensagem != null)
        {
            ICanal canalSelecionado = Factory.CriarCanal(ObterNomeCanal(escolhaCanal));
            canalSelecionado.EnviarMensagem(destinatario, mensagem);
        }
    }

    static string ObterNomeCanal(int escolha)
    {
        switch (escolha)
        {
            case 1:
                return "whatsapp";
            case 2:
                return "telegram";
            case 3:
                return "facebook";
            case 4:
                return "instagram";
            default:
                return null;
        }
    }

    static MensagemVideo CriarMensagemVideo(string conteudo)
    {
        Console.Write("Digite o arquivo: ");
        string arquivo = Console.ReadLine();

        Console.Write("Digite o formato: ");
        string formato = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(arquivo) || string.IsNullOrWhiteSpace(formato))
        {
            Console.WriteLine("Arquivo e formato são obrigatórios. Abortando criação de mensagem de vídeo.");
            return null;
        }

        int duracao;
        Console.Write("Digite a duração (em minutos): ");
        if (!int.TryParse(Console.ReadLine(), out duracao) || duracao <= 0)
        {
            Console.WriteLine("Duração inválida. Usando duração padrão de 0 minutos.");
            duracao = 0;
        }

        return new MensagemVideo { Conteudo = conteudo, DataEnvio = DateTime.Now, Arquivo = arquivo, Formato = ParseFormato(formato), Duracao = duracao };
    }

    static TiposDeArquivo ParseFormato(string formato)
    {
        if (Enum.TryParse(formato, true, out TiposDeArquivo tipoArquivo))
        {
            return tipoArquivo;
        }
        else
        {
            Console.WriteLine($"Formato '{formato}' não reconhecido. Usando formato padrão.");
            return TiposDeArquivo.MP4;
        }
    }
}
