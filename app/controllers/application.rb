# Filters added to this controller apply to all controllers in the application.
# Likewise, all the methods added will be available for all controllers.

class ApplicationController < ActionController::Base
  include AuthenticatedSystem
  helper :all # include all helpers, all the time

  # See ActionController::RequestForgeryProtection for details
  # Uncomment the :secret if you're not using the cookie session store
  protect_from_forgery # :secret => 'c7544bed0f5f8eafcd3fe758631ec30f'
  
  # See ActionController::Base for details 
  # Uncomment this to filter the contents of submitted sensitive data parameters
  # from your application log (in this case, all fields with names like "password"). 
  # filter_parameter_logging :password
  
  
  helper_method :admin?, :user_id

  protected

  def authorize
    unless admin?
      flash[:error] = "unauthorized access"
      redirect_to home_path
      false
    end
  end

  def admin?
    #current_user.admin?
    administrators = ["distaula"]
    if logged_in?
      administrators.include?(current_user.login)
    else
      false
    end
  end

  def user_id
    if logged_in?
      current_user.login
    else
      request.remote_ip
    end
  end
  
end
