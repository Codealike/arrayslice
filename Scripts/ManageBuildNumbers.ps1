function Get-BuildVersion  
{  
    [CmdletBinding()]  
    param()  
          
    $assemblyPattern = "[0-9]+(\.([0-9]+|\*)){1,3}"  
    $assemblyVersionPattern = 'AssemblyVersion\("([0-9]+(\.([0-9]+|\*)){1,3})"\)'  
      
    $foundFiles = get-childitem .\ArraySlice.Fody\*AssemblyInfo.cs -recurse  
                         
              
    $rawVersionNumberGroup = get-content $foundFiles | select-string -pattern $assemblyVersionPattern | select -first 1 | % { $_.Matches }              
    $rawVersionNumber = $rawVersionNumberGroup.Groups[1].Value  
     
    $versionParts = $rawVersionNumber.Split('.')       
    $updatedAssemblyVersion = "{0}.{1}.{2}" -f $versionParts[0], $versionParts[1], $versionParts[2] 
      
    return $updatedAssemblyVersion
} 