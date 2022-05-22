Imports System.IO
Module Deleter

    Sub Main()
        If File.Exists("files_to_delete.txt") = False Then
            Console.WriteLine("creating files to delete list")
            Try
                Dim sw As New StreamWriter("files_to_delete.txt")
                sw.Close()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End If

        If File.Exists("directories_to_delete.txt") = False Then
            Console.WriteLine("creating directories to delete list")
            Try
                Dim sw As New StreamWriter("directories_to_delete.txt")
                sw.Close()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End If

        Try
            Dim sr As New StreamReader("files_to_delete.txt")
            Do Until sr.EndOfStream
                Dim file_name As String = sr.ReadLine
                file_name = correct_path(file_name)
                If File.Exists(file_name) Then
                    Try
                        File.Delete(file_name)
                        Console.WriteLine("file deleted: {0}", file_name)
                    Catch ex As Exception
                        Console.WriteLine("ERROR deleting FILE: {0}", file_name)
                    End Try
                End If
            Loop
            sr.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Try
            Dim sr As New StreamReader("directories_to_delete.txt")
            Do Until sr.EndOfStream
                Dim dir_name As String = sr.ReadLine
                dir_name = correct_path(dir_name)
                If Directory.Exists(dir_name) Then
                    Try
                        Dim files() As String = Directory.GetFiles(dir_name)
                        For Each fil As String In files
                            Try
                                File.Delete(fil)
                            Catch ex As Exception
                                Console.WriteLine("ERROR deleting FILE : {0}", fil)
                            End Try
                        Next
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                    Try
                        Directory.Delete(dir_name)
                        Console.WriteLine("Directory deleted: {0}", dir_name)
                    Catch ex As Exception
                        Console.WriteLine("ERROR deleting DIRECTORY: {0}", dir_name)
                    End Try
                End If
            Loop
            sr.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Console.ReadLine()
    End Sub
    Private Function correct_path(ByRef path As String) As String
        correct_path = path.ToLower
        If correct_path.Contains("%windir%") Then
            correct_path = correct_path.Replace("%windir%", Environment.GetFolderPath(Environment.SpecialFolder.Windows))
        End If

        If correct_path.Contains("%roaming%") Then
            correct_path = correct_path.Replace("%roaming%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        End If

        If correct_path.Contains("%local%") Then
            correct_path = correct_path.Replace("%local%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
        End If

        If correct_path.Contains("%%programdata%") Then
            correct_path = correct_path.Replace("%%programdata%", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))
        End If

    End Function
End Module
