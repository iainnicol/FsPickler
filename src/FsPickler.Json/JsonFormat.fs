﻿namespace Nessos.FsPickler

    open System.IO

    type JsonPickleFormatProvider (indent, omitHeader) =
        
        member val Indent = indent with get,set
        member val OmitHeader = omitHeader with get,set

        interface ITextPickleFormatProvider with
            member __.Name = "Json"

            member __.CreateWriter (stream, encoding, leaveOpen) =
                let sw = new StreamWriter(stream, encoding, 1024, leaveOpen)
                new JsonPickleWriter(sw, __.OmitHeader, __.Indent, leaveOpen) :> _

            member __.CreateReader (stream, encoding, leaveOpen) =
                let sr = new StreamReader(stream, encoding, true, 1024, leaveOpen)
                new JsonPickleReader(sr, __.OmitHeader, leaveOpen) :> _

            member __.CreateWriter (textWriter, leaveOpen) = 
                new JsonPickleWriter(textWriter, __.OmitHeader, __.Indent, leaveOpen) :> _

            member __.CreateReader (textReader, leaveOpen) = 
                new JsonPickleReader(textReader, __.OmitHeader, leaveOpen) :> _