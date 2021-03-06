Add-Type -AssemblyName System.Windows.Forms;

function ConvertFrom-SecureToPlain {
    
    param( [Parameter(Mandatory=$true)][System.Security.SecureString] $SecurePassword)
    
    # Create a "password pointer".
    $PasswordPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
    
    # Get the plain text version of the password.
    $PlainTextPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto($PasswordPointer)
    
    # Free the pointer.
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($PasswordPointer)
    
    # Return the plain text password.
    $PlainTextPassword
}


Write-Host "";
Write-Host "";
Write-Host "****";
Write-Host "This is Clients script run update for LOCAL Sana Live server";
Write-Host "****";

$CurrentDirectory = (Resolve-Path .\).Path;

$FileBrowser = New-Object System.Windows.Forms.OpenFileDialog -Property @{ 
    InitialDirectory = $CurrentDirectory
    Filter = 'Sql Scripts (*.sql)|*.sql'
}

$DialogResult = $FileBrowser.ShowDialog();

If ($DialogResult -eq [System.Windows.Forms.DialogResult]::OK) {
	$FileName = $FileBrowser.FileName;
	
	$DbUpdatesPath = "..\..\DbUpdates_Build\Windows\netcoreapp2.0\DbUpdates.dll";
	$DbNameTemplate = "Client";
	
    $ServerAddress = Read-Host "Enter your local server address";
    $UserId = Read-Host "Enter your local postgres server user id";

	# Password for beta is lskuscs1d80k
	$SecurePassword = Read-Host "Enter LOCAL PostgreSQL Server password" -AsSecureString;
	$PlainTextPassword = ConvertFrom-SecureToPlain $SecurePassword;
	
	$ConnectionString = "Server=$ServerAddress;Port=5432;Database=postgres;User Id=$UserId;Password=$PlainTextPassword;"
	
	dotnet $DbUpdatesPath -cs $ConnectionString -rs $FileName -db $DbNameTemplate;
}