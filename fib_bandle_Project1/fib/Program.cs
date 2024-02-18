using System.CommandLine;


var rootCommand = new RootCommand("Root command for File Bundler CLI");
var bundleCommand = new Command("bundle", "bundle code files to a signale file");
var responseCommand = new Command("create-rsp", "response ");

var outputOption = new Option<FileInfo>("--output", "File path and Name");
var languageOption = new Option<string>("--language", "One or more programming languages");
var noteOption = new Option<bool>("--note", "Comment with the source code - File path and Name");
var sortOption = new Option<String>("--sort", "order of copying the files,by alphabet name or type of code");
var removeEmptyLinesOption = new Option<bool>("--rel", "remove empty lines");
var authorOption = new Option<string>("--author", "File path and Name");

rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(responseCommand);

//rootCommand.AddOption(outputOption);
bundleCommand.AddOption(outputOption);
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removeEmptyLinesOption);
bundleCommand.AddOption(authorOption);

languageOption.IsRequired = true;
sortOption.SetDefaultValue("alphabet");
authorOption.SetDefaultValue("null");

bundleCommand.SetHandler((output,language , note, sort, rel, author) =>
{
    try
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] files = Directory.GetFiles(currentDirectory, ".", SearchOption.AllDirectories);

        FileStream fs = new FileStream(output.FullName,
                FileMode.Create,
                FileAccess.Write);
        StreamWriter writer = new StreamWriter(fs);
        
        //author option
        if (author != "null")
        {
            writer.WriteLine("author: "+ author);
        }

        //sort option
        if (sort == "alphabet")
        {
            Array.Sort(files);
        }
        else if (sort == "type")
        {
            files.OrderBy(filePath => Path.GetExtension(filePath)).ToArray();
        }

        foreach (string file in files)
        {
            string[] languagesArr = language.Split(' ');
            
            //language option
            if (language=="all" || languagesArr.Contains(file.Split('.')[1]))
            {
                writer.WriteLine(" ");

                //note option
                if (note == true)
                {
                    writer.WriteLine("//source code: ./" + Path.GetRelativePath(currentDirectory, file));
                }
                
                //remove empty line option
                if (rel) { 
                    string[] lines = File.ReadAllLines(file);
                    string[] nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                    File.WriteAllLines(file, nonEmptyLines);

                }
                FileStream fs2 = new FileStream(file,
                    FileMode.Open,
                    FileAccess.Read);
                StreamReader reader = new StreamReader(fs2);
                string line = reader.ReadToEnd();
                writer.WriteLine(line);
                reader.Close();
                fs2.Close();
            }      
        }
        writer.Close();
        fs.Close();
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("Error: File path is invalid, Please enter correct path");
    }
}, outputOption, languageOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);



responseCommand.SetHandler(() =>
{
    Console.Write("your name: ");
    string author = Console.ReadLine();
    Console.WriteLine("response file name: ");
    string resname = Console.ReadLine();
    Console.Write("path and name of the new file: ");
    string output = Console.ReadLine();
    Console.Write("language: ");
    string language = Console.ReadLine();
    Console.Write("note (true/false): ");
    string note = Console.ReadLine();
    Console.Write("sort (alpbetic/type): ");
    string sort = Console.ReadLine();
    Console.Write("remove empty lines (true/false): ");
    string rel = Console.ReadLine();
    String fib = "bundle " + "--output " + output + " --author " + author + " --language " + language +" --note "+ note +" --rel "+ rel+ " --sort "+ sort;
    File.WriteAllText(resname,fib);
});
rootCommand.InvokeAsync(args);