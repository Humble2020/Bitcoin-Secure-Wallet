Imports System.IO
Imports NBitcoin

Public Class Form1
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'create
        Try
            Xseed = Me.TextBox2.Text
            Xname = Me.TextBox3.Text
            Xpassword = Me.TextBox4.Text
            'create wallet..
            'use seed and create bitcoin wallet
            If Me.TextBox3.Text = String.Empty AndAlso Me.TextBox4.Text = String.Empty Then
                MsgBox($"Please give a Name to your Newly created wallet!. {vbNewLine + vbNewLine} Also Provide a password for Extra security to your wallet. {vbNewLine + vbNewLine}Thank You!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Name and Password Important")
                Exit Sub
            End If
            If Directory.Exists(bitcoinWalletBacked_location & "\" & Xname) Then
                MessageBox.Show("Please change the name. Its identical with previous wallet name.", "Wallet name Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Exit Sub
            ElseIf Not Directory.Exists(bitcoinWalletBacked_location & "\" & Xname) Then
                Directory.CreateDirectory(bitcoinWalletBacked_location & "\" & Xname)
            End If
            'Form1.Mypasswd = Me.TextBox4.Text
            'MySeedL = Me.TextBox2.Text
            Try
                OOH_Path = bitcoinWalletBacked_location & "\" & Xname & "\9_walT.9J1Q"
                '    FinishCreatingFile("ecrypted", OOH_Path)
                Dim extKey As ExtKey = generateMasterAdress(seed)
                BitcoinSecret = generateDerivedAdress(extKey, bitcoinSecrets.Count())
                inputReceiving.Text = BitcoinSecret.PubKey.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet).ToString()

            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub
    Public Sub addPrivateKey(ByVal bs As BitcoinSecret)
        bitcoinSecrets.Add(bs)
    End Sub
    Public Function getSecrets() As BitcoinSecret()
        Return bitcoinSecrets.ToArray()
    End Function
    Public Function getAddresses() As List(Of AddressValue)
        Dim adressValues As New List(Of AddressValue)()
        For i As Integer = 0 To bitcoinSecrets.Count - 1
            adressValues.Add(New AddressValue(bitcoinSecrets(i).GetAddress(ScriptPubKeyType.Legacy).ToString()))
        Next i
        Return adressValues
    End Function
    Public Shared Function generateDerivedAdress(ByVal masterKey As ExtKey, ByVal i As Integer) As BitcoinSecret
        Dim keypth As New KeyPath("m/44'/0'/0'/0/" & i) 'change last 0
        Dim childKey As ExtKey = masterKey.Derive(keypth) '0 receiving
        Return childKey.PrivateKey.GetBitcoinSecret(Network.Main) '1 change
    End Function
    Dim Xseed As String = Nothing
    Dim Xname As String = Nothing
    Dim Xpassword As String = Nothing
    Public ssMnemo As String
    Public mnemonic As Mnemonic
    Public nameOf_wallet As String = Nothing
    Public bitcoinWalletBacked_location As String = Application.StartupPath & "\PD_Wallet"
    Public hdRoot As New ExtKey()
    Public OOH_Path As String 'o
    Public bitcoinSecrets As New List(Of BitcoinSecret)()
    Dim BitcoinSecret As BitcoinSecret
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        Check_internet_Timer.Start()
        Try
            mnemonic = New Mnemonic(Wordlist.English, WordCount.TwentyFour)
            ssMnemo = mnemonic.ToString()
            Me.Invoke(New MethodInvoker(Sub() Me.TextBox2.Text = ssMnemo))
        Catch ex As Exception

        End Try
    End Sub
    Public Shared Function checkAdress(ByVal publicAdress As String) As Boolean
        Try
            Dim publicKey As BitcoinAddress = BitcoinAddress.Create(publicAdress, Network.Main)
            Return True
        Catch
            Return False
        End Try
    End Function
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'random
        Try
            mnemonic = New Mnemonic(Wordlist.English, WordCount.TwentyFour)
            ssMnemo = mnemonic.ToString()
            Me.Invoke(New MethodInvoker(Sub() Me.TextBox2.Text = ssMnemo))
            Dim adressValid As Boolean
            adressValid = checkAdress("bitcoin address")

        Catch ex As Exception

        End Try
    End Sub
    Public Shared Function generateMasterAdress(Optional ByVal seed As String = "", Optional ByVal passwd As String = "") As ExtKey
        'INSTANT VB NOTE: The variable Seed was renamed since Visual Basic will not allow local variables with the same name as parameters or other local variables:
        Dim passw As String = Nothing
        If Not passwd = String.Empty Then
            passw = passwd
        End If
        Dim Seed_Conflict As Mnemonic
        If seed = "" Then
            Seed_Conflict = New Mnemonic(Wordlist.English, WordCount.Twelve)
        Else
            Seed_Conflict = New Mnemonic(seed, Wordlist.English)
        End If
        Dim masterKey As New ExtKey()
        masterKey = Seed_Conflict.DeriveExtKey(passw)
        Return masterKey.GetWif(Network.Main)
    End Function

    Sub AnotherWallet()
        mnemonic = New Mnemonic(Wordlist.English, WordCount.TwentyFour)
        ssMnemo = mnemonic.ToString()
        Me.Invoke(New MethodInvoker(Sub() Me.TextBox2.Text = ssMnemo))

        Dim hdRoot As New ExtKey()
        hdRoot = mnemonic.DeriveExtKey(Xpassword)
        ''  txtRoot.Text = hdRoot.ToString(Network.Main)

        Select Case cboCoin.SelectedValue
            Case "BTC - Bitcoin (BIP-44)"
                Dim key44 As ExtKey = hdRoot.Derive(New NBitcoin.KeyPath("m/44'/0'/" & cboAccount.SelectedValue & "'/" & Convert.ToString(cboType.SelectedValue).Substring(0, 1) & "/" & cboIndex.SelectedValue))
                Dim pubKey44 As ExtPubKey = key44.Neuter()
                txtAddress.Text = key44.PrivateKey.PubKey.ToString(Network.Main)
                txtPublicKey.Text = Convert.ToString(pubKey44.PubKey.ScriptPubKey).Substring(0, 66)
                txtExtPublicKey.Text = pubKey44.ToString(Network.Main)
                txtPrivateKey.Text = key44.PrivateKey.ToString(Network.Main)


            'key32 = hdRoot.Derive(New NBitcoin.KeyPath("m/44'/0'/" & cboAccount.SelectedValue & "'/" & Convert.ToString(cboType.SelectedValue).Substring(0, 1)))
            'pubKey32 = key32.Neuter()
            'txt32ExtPublicKey.Text = pubKey32.ToString(Network.Main)

            Case "ETH - Ethereum (BIP-44)"
                Dim index As Integer = Convert.ToInt32(cboIndex.SelectedValue)
                Dim account = wallet.GetAccount(index)
                txtAddress.Text = account.Address
                txtPublicKey.Text = ""
                txtExtPublicKey.Text = ""
                txtPrivateKey.Text = account.PrivateKey

            Case "BTC - Bitcoin (BIP-49)"
                Dim key49 As ExtKey = hdRoot.Derive(New NBitcoin.KeyPath("m/49'/0'/" & cboAccount.SelectedValue & "'/" & Convert.ToString(cboType.SelectedValue).Substring(0, 1) & "/" & cboIndex.SelectedValue))
                Dim pubKey49 As ExtPubKey = key49.Neuter()
                txtAddress.Text = key49.PrivateKey.PubKey.WitHash.GetAddress(Network.Main).GetScriptAddress().ToString()
                txtPublicKey.Text = Convert.ToString(pubKey49.PubKey.ScriptPubKey).Substring(0, 66)
                txtExtPublicKey.Text = pubKey49.ToString(Network.Main)
                txtPrivateKey.Text = key49.PrivateKey.ToString(Network.Main)
                key32 = hdRoot.Derive(New NBitcoin.KeyPath("m/49'/0'/" & cboAccount.SelectedValue & "'/" & Convert.ToString(cboType.SelectedValue).Substring(0, 1)))
                pubKey32 = key32.Neuter()
                txt32ExtPublicKey.Text = pubKey32.ToString(Network.Main)
        End Select

    End Sub
End Class
Public Class AddressValue
    Public Sub New(ByVal adress As String)
        _address = adress
    End Sub
    Public Property Address() As String
        Get
            Return _address
        End Get
        Set(ByVal value As String)
            _address = value
        End Set
    End Property
    Private _address As String
End Class
