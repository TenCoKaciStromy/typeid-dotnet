// See https://aka.ms/new-console-template for more information

using TcKs.TypeId;
using static System.Console;

if (args.Length < 1)
  return PrintHelpForNoArgs();

return args[0] switch {
  "new" => PrintNew(args),
  "decode" => PrintDecode(args),
  "encode" => PrintEncode(args),
  "help" or "-help" or "--help" or "/help"
    or "?" or "-?" or "--?" or "/?"
    or "h" or "-h"
    => PrintHelp(),
_ => PrintHelpForNotSupportedCommand(args)
};

static int PrintNew(string[] args)
  => args.Length != 2
    ? PrintHelpForInvalidCommandParametersCount(args)
    : PrintError(TypeId.NewId(args[1]));

static int PrintDecode(string[] args)
  => args.Length != 2
    ? PrintHelpForInvalidCommandParametersCount(args)
    : TypeId.TryParse(args[1], out var typeId)
      ? PrintOk($"type: {typeId.Type}\nuuid: {typeId.Id}")
      : PrintError("Decode failed. Can not parse input.");

static int PrintEncode(string[] args)
  => args.Length != 3
    ? PrintHelpForInvalidCommandParametersCount(args)
    : Guid.TryParse(args[2], out var uuid)
      ? PrintOk(new TypeId(args[1], uuid))
      : PrintError("Encode failed. Can not parse input uuid.");

static int PrintHelpForNoArgs()
  => PrintError("No arguments was passed.");

static int PrintHelpForInvalidCommandParametersCount(string[] args)
  => PrintError($"Command ({args[0]}) has invalid count of parameters.");

static int PrintHelpForNotSupportedCommand(string[] args)
  => PrintError($"Command ({args[0]}) is not supported.");

static int PrintHelp()
  => PrintOk("TODO: print help");

static int PrintOk(object valueToPrint)
  => Print(0, valueToPrint);

static int PrintError(object errorToPrint)
  => Print(-1, errorToPrint);
  
static int Print(int returnCode, object valueToPrint) {
  WriteLine(valueToPrint);
  return returnCode;
}