pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:1.1-sdk'
      args '-u root'
    }
  }
  options {
    timestamps()
  }
  
  stages {
    stage('Restore') {
      steps {
        sh 'dotnet restore'
      }
    }
    stage('Build') {
      steps {
        sh 'dotnet build -c Release'
      }
    }
    stage('Pack') {
      steps {
        sh 'dotnet pack -c Release -o "$(pwd)/artifacts"'
      }
    }
    stage('Deploy') {
      when  { branch 'master' }
      environment {
        NUGET_API_KEY = credentials('NuGet')
      }
      steps {
        sh 'echo "${NUGET_API_KEY}"'
        // sh 'dotnet nuget push artifacts/*.nupkg -k "${NUGET_API_KEY}" --no-symbols'
      }
    }
  }
}
