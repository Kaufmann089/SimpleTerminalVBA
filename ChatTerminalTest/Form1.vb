


'http://msdn.microsoft.com/de-de/library/vstudio/system.io.ports.serialport(v=vs.100).aspx
'Im folgenden Codebeispiel wird veranschaulicht, wie es zwei Benutzern mithilfe der SerialPort-Klasse ermöglicht wird, auf zwei getrennten Computern, die durch ein Null-Modem-Kabel verbunden sind, miteinander zu chatten. In diesem Beispiel werden die Benutzer vor dem Chatten zur Angabe der Anschlusseinstellungen und des Benutzernamens aufgefordert. Um den vollen Funktionsumfang des Beispiels nutzen zu können, muss das Programm auf beiden Computern ausgeführt werden.

Imports System
Imports System.IO.Ports
Imports System.Threading



Public Class Form1
    Dim _serialPort As New SerialPort()

    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim a As String

        'Dim _serialPort As New SerialPort()
        ' Create a new SerialPort object with default settings.

        If _serialPort.IsOpen = False Then
            _serialPort.PortName = "COM2"
            _serialPort.Parity = Parity.None
            _serialPort.BaudRate = 9600
            _serialPort.StopBits = 1
            _serialPort.Handshake = Handshake.None
            _serialPort.Open()
            Console.WriteLine("COM ist geoeffnet")
        End If


        If _serialPort.IsOpen Then

            '_serialPort.WriteLine("ESET")
            a = Me.TxttoSend.Text & vbCrLf
            _serialPort.WriteLine(a)
            Thread.Sleep(1000) 'wait 1 sec
            'a = _serialPort.ReadExisting()
            a = _serialPort.ReadLine
            Console.WriteLine("Gelesen: {0}", a)

        End If
    End Sub
End Class



Public Class PortChat
    Shared _continue As Boolean
    Shared _serialPort As SerialPort

    Public Shared Sub Main()
        Dim name As String
        Dim message As String
        Dim sComparer As StringComparer = StringComparer.OrdinalIgnoreCase
        Dim readThread As Thread = New Thread(AddressOf Read)

        ' Create a new SerialPort object with default settings.
        _serialPort = New SerialPort()

        ' Allow the user to set the appropriate properties.
        _serialPort.PortName = SetPortName(_serialPort.PortName)
        _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate)
        _serialPort.Parity = SetPortParity(_serialPort.Parity)
        _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits)
        _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits)
        _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake)

        ' Set the read/write timeouts
        _serialPort.ReadTimeout = 500
        _serialPort.WriteTimeout = 500

        _serialPort.Open()
        _continue = True
        readThread.Start()

        Console.Write("Name: ")
        name = Console.ReadLine()

        Console.WriteLine("Type QUIT to exit")

        While (_continue)
            message = Console.ReadLine()

            If sComparer.Equals("quit", message) Then
                _continue = False
            Else
                _serialPort.WriteLine( _
                    String.Format("<{0}>: {1}", name, message))
            End If
        End While

        readThread.Join()
        _serialPort.Close()
    End Sub

    Public Shared Sub Read()
        While (_continue)
            Try
                Dim message As String = _serialPort.ReadLine()
                Console.WriteLine(message)
            Catch ex As TimeoutException
                ' Do nothing
            End Try
        End While
    End Sub

    Public Shared Function SetPortName(ByVal defaultPortName As String) As String
        Dim newPortName As String

        Console.WriteLine("Available Ports:")
        Dim s As String
        For Each s In SerialPort.GetPortNames()
            Console.WriteLine("   {0}", s)
        Next s

        Console.Write("COM port({0}): ", defaultPortName)
        newPortName = Console.ReadLine()

        If newPortName = "" Then
            newPortName = defaultPortName
        End If
        Return newPortName
    End Function

    Public Shared Function SetPortBaudRate(ByVal defaultPortBaudRate As Integer) As Integer
        Dim newBaudRate As String

        Console.Write("Baud Rate({0}): ", defaultPortBaudRate)
        newBaudRate = Console.ReadLine()

        If newBaudRate = "" Then
            newBaudRate = defaultPortBaudRate.ToString()
        End If

        Return Integer.Parse(newBaudRate)
    End Function

    Public Shared Function SetPortParity(ByVal defaultPortParity As Parity) As Parity
        Dim newParity As String

        Console.WriteLine("Available Parity options:")
        Dim s As String
        For Each s In [Enum].GetNames(GetType(Parity))
            Console.WriteLine("   {0}", s)
        Next s

        Console.Write("Parity({0}):", defaultPortParity.ToString())
        newParity = Console.ReadLine()

        If newParity = "" Then
            newParity = defaultPortParity.ToString()
        End If

        Return CType([Enum].Parse(GetType(Parity), newParity), Parity)
    End Function

    Public Shared Function SetPortDataBits(ByVal defaultPortDataBits As Integer) As Integer
        Dim newDataBits As String

        Console.Write("Data Bits({0}): ", defaultPortDataBits)
        newDataBits = Console.ReadLine()

        If newDataBits = "" Then
            newDataBits = defaultPortDataBits.ToString()
        End If

        Return Integer.Parse(newDataBits)
    End Function

    Public Shared Function SetPortStopBits(ByVal defaultPortStopBits As StopBits) As StopBits
        Dim newStopBits As String

        Console.WriteLine("Available Stop Bits options:")
        Dim s As String
        For Each s In [Enum].GetNames(GetType(StopBits))
            Console.WriteLine("   {0}", s)
        Next s

        Console.Write("Stop Bits({0}):", defaultPortStopBits.ToString())
        newStopBits = Console.ReadLine()

        If newStopBits = "" Then
            newStopBits = defaultPortStopBits.ToString()
        End If

        Return CType([Enum].Parse(GetType(StopBits), newStopBits), StopBits)
    End Function

    Public Shared Function SetPortHandshake(ByVal defaultPortHandshake As Handshake) As Handshake
        Dim newHandshake As String

        Console.WriteLine("Available Handshake options:")
        Dim s As String
        For Each s In [Enum].GetNames(GetType(Handshake))
            Console.WriteLine("   {0}", s)
        Next s

        Console.Write("Handshake({0}):", defaultPortHandshake.ToString())
        newHandshake = Console.ReadLine()

        If newHandshake = "" Then
            newHandshake = defaultPortHandshake.ToString()
        End If

        Return CType([Enum].Parse(GetType(Handshake), newHandshake), Handshake)
    End Function
End Class

