

pipeline {
    agent any

    environment {
        IMAGE_NAME = 'paramveer03/devopscrudapp'
	CONTAINER_NAME = 'devopscrudappcont'
        IMAGE_TAG = 'latest'
	 SCANNER_HOME= tool 'sonar-scanner'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build DevopsCrudapplication/DevopsCrudapplication.csproj --configuration Release'
            }
        }

	stage('Unit Tests') {
            steps {
                // Run unit tests for the .NET project
                dir('DevopsCrudapplication.Tests/bin/Debug/net7.0') {
                    sh 'dotnet test DevopsCrudapplication.Tests.dll'
                }
            }
        }

	stage('Sonarqube Analysis') {
            steps {
                withSonarQubeEnv('sonar-server'){
                    sh ''' $SCANNER_HOME/bin/sonar-scanner -Dsonar.projectName=DevopsCrudapplication \
                    -Dsonar.java.binaries=. \
                    -Dsonar.projectKey=DevopsCrudapplication '''
                }
            }
        }

      

        stage('Docker Build') {
            steps {
                // Build your Docker image
                script {
                    docker.build("${IMAGE_NAME}:${IMAGE_TAG}")
                }
            }
        }

        stage('Docker Push') {
            steps {
                script {
                    // Login to Docker Hub (or your Docker registry)
                    // Make sure to set your credentials in Jenkins credential store
                    docker.withRegistry('https://index.docker.io/v1/', 'dockerlogin') {
                        // Push your Docker image
                        docker.image("${IMAGE_NAME}:${IMAGE_TAG}").push()
                    }
                }
            }
        }

        stage('Run Container') {
            steps {
                script {
                    // Stop the currently running container (if any)
                    sh 'docker rm -f ${CONTAINER_NAME} || true'
                    // Run the new container
                    sh "docker run -d --name ${CONTAINER_NAME} -p 8888:80 ${IMAGE_NAME}:${IMAGE_TAG}"
                }
            }
        }
    }
}
