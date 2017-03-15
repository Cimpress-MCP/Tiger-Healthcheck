pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:1.1-sdk'
      args '-u root'
    }
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
  }
}
